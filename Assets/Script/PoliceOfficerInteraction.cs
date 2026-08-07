using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// Controls the player's interaction and conversation with the police officer.
/// It displays dialogue choices, shows the Game Over panel,
/// and moves the PlayerCapsule to the next mission spawn point.
/// </summary>
public class PoliceOfficerInteraction : MonoBehaviour
{
    [Header("Officer Reference")]
    [SerializeField] private PoliceOfficer policeOfficer;

    [Header("Interaction UI")]
    [SerializeField] private GameObject interactionPrompt;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;

    [Header("First Response Buttons")]
    [SerializeField] private GameObject responseButtons;
    [SerializeField] private Button okayButton;
    [SerializeField] private Button whyButton;

    [Header("Second Response Button")]
    [SerializeField] private GameObject understandButtonObject;
    [SerializeField] private Button understandButton;

    [Header("Game Over UI")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Button nextMissionButton;

    [Header("Next Mission")]
    [SerializeField] private Transform nextSpawnPoint;

    [Header("Player Control")]
    [SerializeField] private Behaviour[] playerControlsToDisable;

    [Header("Interaction")]
    [SerializeField] private float interactionDistance = 3f;

    [Header("Dialogue")]
    [TextArea(2, 5)]
    [SerializeField]
    private string firstOfficerDialogue =
        "Police Officer: Excuse me, did you know that jaywalking is wrong? " +
        "It can cause an accident. I'll let you off with a warning, " +
        "but next time, don't do it again.";

    [TextArea(2, 5)]
    [SerializeField]
    private string explanationDialogue =
        "Police Officer: Jaywalking can cause an accident because some " +
        "drivers may be unable to brake their cars in time, or they may " +
        "not notice you crossing the road.";

    private bool dialogueStarted;
    private bool conversationCompleted;

    /// <summary>
    /// Sets up all button events and hides the dialogue UI
    /// when the scene begins.
    /// </summary>
    private void Start()
    {
        HideAllUI();

        if (okayButton != null)
            okayButton.onClick.AddListener(ChooseOkay);

        if (whyButton != null)
            whyButton.onClick.AddListener(ChooseWhy);

        if (understandButton != null)
            understandButton.onClick.AddListener(ChooseUnderstand);

        if (nextMissionButton != null)
            nextMissionButton.onClick.AddListener(GoToNextMission);
    }

    /// <summary>
    /// Checks whether the player is close enough to the police officer
    /// and allows the conversation to begin by pressing E.
    /// </summary>
    private void Update()
    {
        if (policeOfficer == null)
            return;

        if (conversationCompleted)
            return;

        if (!policeOfficer.CanInteract)
        {
            HideInteractionPrompt();
            return;
        }

        float distance = Vector3.Distance(
            transform.position,
            policeOfficer.transform.position
        );

        bool playerIsNearOfficer =
            distance <= interactionDistance;

        if (!playerIsNearOfficer)
        {
            HideInteractionPrompt();
            return;
        }

        if (!dialogueStarted && interactionPrompt != null)
            interactionPrompt.SetActive(true);

        if (Keyboard.current == null)
            return;

        if (!dialogueStarted &&
            Keyboard.current.eKey.wasPressedThisFrame)
        {
            StartConversation();
        }
    }

    /// <summary>
    /// Starts the conversation, shows the dialogue,
    /// unlocks the cursor, and disables player movement.
    /// </summary>
    private void StartConversation()
    {
        dialogueStarted = true;

        EnterDialogueMode();

        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);

        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);

        if (dialogueText != null)
            dialogueText.text = firstOfficerDialogue;

        if (responseButtons != null)
            responseButtons.SetActive(true);

        if (understandButtonObject != null)
            understandButtonObject.SetActive(false);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    /// <summary>
    /// Unlocks the cursor and disables the assigned
    /// player movement and camera-control scripts.
    /// </summary>
    private void EnterDialogueMode()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        foreach (Behaviour playerControl in playerControlsToDisable)
        {
            if (playerControl != null)
                playerControl.enabled = false;
        }
    }

    /// <summary>
    /// Restores the player movement scripts and locks
    /// the mouse cursor for normal gameplay.
    /// </summary>
    private void ExitDialogueMode()
    {
        foreach (Behaviour playerControl in playerControlsToDisable)
        {
            if (playerControl != null)
                playerControl.enabled = true;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    /// Handles the player's "Okay..." response
    /// and shows the Game Over panel.
    /// </summary>
    public void ChooseOkay()
    {
        ShowGameOverPanel();
    }

    /// <summary>
    /// Handles the player's "But why?" response
    /// and displays the police officer's explanation.
    /// </summary>
    public void ChooseWhy()
    {
        if (dialogueText != null)
            dialogueText.text = explanationDialogue;

        if (responseButtons != null)
            responseButtons.SetActive(false);

        if (understandButtonObject != null)
            understandButtonObject.SetActive(true);
    }

    /// <summary>
    /// Handles the player's "Okay, I understand" response
    /// and shows the Game Over panel.
    /// </summary>
    public void ChooseUnderstand()
    {
        ShowGameOverPanel();
    }

    /// <summary>
    /// Hides the dialogue UI and shows the Game Over panel.
    /// </summary>
    private void ShowGameOverPanel()
    {
        conversationCompleted = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        if (responseButtons != null)
            responseButtons.SetActive(false);

        if (understandButtonObject != null)
            understandButtonObject.SetActive(false);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    /// <summary>
    /// Moves the PlayerCapsule to the next spawn point,
    /// hides the Game Over panel, and restores player control.
    /// </summary>
    public void GoToNextMission()
    {
        if (nextSpawnPoint == null)
        {
            Debug.LogError(
                "NextSpawnPoint has not been assigned.",
                gameObject
            );

            return;
        }

        CharacterController characterController =
            GetComponent<CharacterController>();

        Rigidbody playerRigidbody =
            GetComponent<Rigidbody>();

        // Temporarily disable CharacterController so Unity
        // does not fight against the teleport position.
        if (characterController != null)
            characterController.enabled = false;

        // Clear Rigidbody movement before teleporting.
        if (playerRigidbody != null)
        {
            playerRigidbody.linearVelocity = Vector3.zero;
            playerRigidbody.angularVelocity = Vector3.zero;
        }

        transform.SetPositionAndRotation(
            nextSpawnPoint.position,
            nextSpawnPoint.rotation
        );

        if (characterController != null)
            characterController.enabled = true;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        if (responseButtons != null)
            responseButtons.SetActive(false);

        if (understandButtonObject != null)
            understandButtonObject.SetActive(false);

        ExitDialogueMode();

        Debug.Log(
            "Player moved to NextSpawnPoint.",
            gameObject
        );
    }

    /// <summary>
    /// Hides the interaction prompt.
    /// </summary>
    private void HideInteractionPrompt()
    {
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);
    }

    /// <summary>
    /// Hides all dialogue and Game Over UI
    /// when the scene begins.
    /// </summary>
    private void HideAllUI()
    {
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        if (responseButtons != null)
            responseButtons.SetActive(false);

        if (understandButtonObject != null)
            understandButtonObject.SetActive(false);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    /// <summary>
    /// Removes the button listeners when this component
    /// is destroyed.
    /// </summary>
    private void OnDestroy()
    {
        if (okayButton != null)
            okayButton.onClick.RemoveListener(ChooseOkay);

        if (whyButton != null)
            whyButton.onClick.RemoveListener(ChooseWhy);

        if (understandButton != null)
            understandButton.onClick.RemoveListener(ChooseUnderstand);

        if (nextMissionButton != null)
            nextMissionButton.onClick.RemoveListener(GoToNextMission);
    }
}