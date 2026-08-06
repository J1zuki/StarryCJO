using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Controls the player's interaction and conversation with the police officer.
/// It displays the interaction prompt, officer dialogue, player response choices,
/// game-over panel, and loads the correct road-crossing scene.
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
    [SerializeField] private Button nextSceneButton;

    [Header("Player Control")]
    [Tooltip("Assign scripts such as Third Person Controller and Starter Assets Inputs.")]
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

    [Header("Next Scene")]
    [SerializeField] private string nextSceneName = "CorrectWayScene";

    private bool dialogueStarted;
    private bool conversationCompleted;

    /// <summary>
    /// Sets up the button events and hides all interaction panels
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

        if (nextSceneButton != null)
            nextSceneButton.onClick.AddListener(LoadNextScene);
    }

    /// <summary>
    /// Checks whether the player is close enough to the police officer
    /// and allows the player to begin the conversation by pressing E.
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
    /// Opens the first officer dialogue, displays the first response choices,
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
    /// Unlocks and shows the mouse cursor, then disables the assigned
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
    /// Locks and hides the mouse cursor, then enables the assigned
    /// player movement and camera-control scripts.
    /// </summary>
    private void ExitDialogueMode()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        foreach (Behaviour playerControl in playerControlsToDisable)
        {
            if (playerControl != null)
                playerControl.enabled = true;
        }
    }

    /// <summary>
    /// Handles the player's "Okay..." response and immediately
    /// displays the game-over panel.
    /// </summary>
    public void ChooseOkay()
    {
        ShowGameOverPanel();
    }

    /// <summary>
    /// Handles the player's "But why? It's convenient." response.
    /// It displays the officer's explanation and shows the
    /// "Okay, I understand" button.
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
    /// and displays the game-over panel.
    /// </summary>
    public void ChooseUnderstand()
    {
        ShowGameOverPanel();
    }

    /// <summary>
    /// Hides the dialogue UI and displays the game-over panel.
    /// The cursor remains visible so the player can click the next-scene button.
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
    /// Restores player control and loads the correct road-crossing scene.
    /// </summary>
    public void LoadNextScene()
    {
        if (string.IsNullOrWhiteSpace(nextSceneName))
        {
            Debug.LogError(
                "The next scene name has not been entered.",
                gameObject
            );

            return;
        }

        if (!Application.CanStreamedLevelBeLoaded(nextSceneName))
        {
            Debug.LogError(
                "The scene '" + nextSceneName +
                "' could not be loaded. Add it to the Build Profile " +
                "and check that the scene name is correct.",
                gameObject
            );

            return;
        }

        ExitDialogueMode();
        SceneManager.LoadScene(nextSceneName);
    }

    /// <summary>
    /// Hides the interaction prompt when the player cannot currently
    /// interact with the police officer.
    /// </summary>
    private void HideInteractionPrompt()
    {
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);
    }

    /// <summary>
    /// Hides all dialogue, response, and game-over UI elements
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
    /// Restores the cursor and player control if the component becomes disabled.
    /// </summary>
    private void OnDisable()
    {
        if (!gameObject.scene.isLoaded)
            return;

        ExitDialogueMode();
    }

    /// <summary>
    /// Removes the button listeners when this component is destroyed
    /// to prevent duplicate or unwanted event calls.
    /// </summary>
    private void OnDestroy()
    {
        if (okayButton != null)
            okayButton.onClick.RemoveListener(ChooseOkay);

        if (whyButton != null)
            whyButton.onClick.RemoveListener(ChooseWhy);

        if (understandButton != null)
            understandButton.onClick.RemoveListener(ChooseUnderstand);

        if (nextSceneButton != null)
            nextSceneButton.onClick.RemoveListener(LoadNextScene);
    }
}