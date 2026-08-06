using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controls the player's interaction with the police officer,
/// including the interaction prompt and warning dialogue UI.
/// </summary>
public class PoliceOfficerInteraction : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PoliceOfficer policeOfficer;
    [SerializeField] private GameObject interactionPrompt;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;

    [Header("Interaction")]
    [SerializeField] private float interactionDistance = 3f;

    [Header("Dialogue")]
    [TextArea(2, 4)]
    [SerializeField]
    private string officerDialogue =
        "Police Officer: If you jaywalk again, you'll get fined.";

    private bool dialogueOpen;

    /// <summary>
    /// Hides the interaction prompt and dialogue panel when the
    /// scene begins.
    /// </summary>
    private void Start()
    {
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }

    /// <summary>
    /// Checks whether the officer is ready for interaction, measures
    /// the player's distance, and handles the E-key input.
    /// </summary>
    private void Update()
    {
        if (policeOfficer == null)
            return;

        if (!policeOfficer.CanInteract)
        {
            HideInteraction();
            return;
        }

        float distance = Vector3.Distance(
            transform.position,
            policeOfficer.transform.position
        );

        bool playerIsNear =
            distance <= interactionDistance;

        if (!playerIsNear)
        {
            HideInteraction();
            return;
        }

        if (!dialogueOpen && interactionPrompt != null)
            interactionPrompt.SetActive(true);

        if (Keyboard.current == null)
            return;

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (dialogueOpen)
                CloseDialogue();
            else
                OpenDialogue();
        }
    }

    /// <summary>
    /// Opens the dialogue panel, hides the interaction prompt,
    /// and displays the police officer's warning message.
    /// </summary>
    private void OpenDialogue()
    {
        dialogueOpen = true;

        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);

        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);

        if (dialogueText != null)
            dialogueText.text = officerDialogue;
    }

    /// <summary>
    /// Closes the dialogue panel and shows the interaction prompt
    /// again while the player remains near the police officer.
    /// </summary>
    private void CloseDialogue()
    {
        dialogueOpen = false;

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        if (interactionPrompt != null)
            interactionPrompt.SetActive(true);
    }

    /// <summary>
    /// Hides both interaction UI elements and resets the dialogue
    /// when the officer is unavailable or the player moves away.
    /// </summary>
    private void HideInteraction()
    {
        dialogueOpen = false;

        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }
}