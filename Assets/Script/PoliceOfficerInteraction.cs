using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

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

    private void Start()
    {
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }

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

    private void CloseDialogue()
    {
        dialogueOpen = false;

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        if (interactionPrompt != null)
            interactionPrompt.SetActive(true);
    }

    private void HideInteraction()
    {
        dialogueOpen = false;

        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }
}