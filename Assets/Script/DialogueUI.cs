/*
 * Author: Cylina Ho & Joyce Kwek
 * Date: 12th August 2026
 * File: DialogueUI.cs
 * Description:
 * Controls the dialogue user interface for NPC interactions.
 * The script opens and closes the dialogue panel,
 * displays dialogue text, manages cursor visibility,
 * and handles the Shop, Talk, and Leave button actions.
 */

using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance { get; private set; }

    [Header("UI Panels & Text")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        
        dialoguePanel.SetActive(false);
    }

    public void OpenDialogue(string greetingText)
    {
        dialogueText.text = greetingText;
        dialoguePanel.SetActive(true);
        
        // Unlock cursor for UI interaction
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseDialogue()
    {
        dialoguePanel.SetActive(false);
        
        // Optional: Re-lock cursor if using first/third person mouse control
        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;
    }

    // Button Listener Methods
    public void OnShopButtonPressed()
    {
        dialogueText.text = "Here's what we have in stock: Slurpees, Instant Ramen, and Energy Drinks!";
        Debug.Log("Opened Shop UI");
    }

    public void OnTalkButtonPressed()
    {
        dialogueText.text = "Shift ends at midnight, but the Slurpee machine never sleeps.";
    }

    public void OnLeaveButtonPressed()
    {
        CloseDialogue();
    }
}