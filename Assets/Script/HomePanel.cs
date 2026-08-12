/*
 * Author: Cylina ho
 * Date: 12th August 2026
 * File: HomePanel.cs
 * Description:
 * Manages the main Home Panel UI.
 * Keeps the home screen open on start, unlocks the mouse cursor,
 * and hides the home screen (and shows instructions/gameplay) 
 * once the player presses the Start button.
 */

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the opening Home Panel screen and handles 
/// starting the game when the Start button is pressed.
/// </summary>
public class HomePanel : MonoBehaviour
{
    [Header("Home UI Setup")]
    [SerializeField] private GameObject homePanel;
    [SerializeField] private Button startButton;

    [Header("Instruction UI Setup")]
    [SerializeField] private GameObject instructionPanel;

    [Header("Audio Setup")]
    [Tooltip("Assign the AudioSource playing your background music (from PlayerCapsule).")]
    [SerializeField] private AudioSource backgroundMusic;

    [Header("Player Control")]
    [Tooltip("Assign player movement scripts to disable while on the home panel.")]
    [SerializeField] private Behaviour[] playerControlsToDisable;

    private void Awake()
    {
        // Bind button listener early in Awake to avoid missing the first click
        if (startButton != null)
        {
            startButton.onClick.RemoveAllListeners();
            startButton.onClick.AddListener(OnStartButtonPressed);
        }
    }

    private void Start()
    {
        if (startButton == null)
        {
            Debug.LogError("HomePanel: Start Button is NOT assigned in the Inspector!", gameObject);
        }

        // Keep home panel open initially
        if (homePanel != null)
        {
            homePanel.SetActive(true);
        }

        // Hide instruction panel until player presses Start
        if (instructionPanel != null)
        {
            instructionPanel.SetActive(false);
        }

        // Ensure background music is stopped or paused at game startup
        if (backgroundMusic != null)
        {
            backgroundMusic.Stop(); // Use Stop instead of Pause so Play() restarts fresh
        }

        DisablePlayerControl();

        // Unlock mouse cursor for UI navigation
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    /// <summary>
    /// Called when the player clicks the Start button.
    /// Hides home screen, plays music, shows instructions, and restores player movement.
    /// </summary>
    public void OnStartButtonPressed()
    {
        // Hide the Home Panel
        if (homePanel != null)
        {
            homePanel.SetActive(false);
        }

        // Show Instruction panel
        if (instructionPanel != null)
        {
            instructionPanel.SetActive(true);
        }

        // Play background music
        if (backgroundMusic != null)
        {
            if (!backgroundMusic.isPlaying)
            {
                backgroundMusic.Play();
            }
        }

        EnablePlayerControl();

        // Lock cursor for gameplay (or keep unlocked if instruction panel requires cursor)
        Cursor.lockState = CursorLockMode.None; // Set to CursorLockMode.Locked when gameplay actually begins
        Cursor.visible = true;

        Debug.Log("Game Started: Home Panel closed, music playing.", gameObject);
    }

    private void DisablePlayerControl()
    {
        if (playerControlsToDisable == null) return;

        foreach (Behaviour playerControl in playerControlsToDisable)
        {
            if (playerControl != null)
            {
                playerControl.enabled = false;
            }
        }
    }

    private void EnablePlayerControl()
    {
        if (playerControlsToDisable == null) return;

        foreach (Behaviour playerControl in playerControlsToDisable)
        {
            if (playerControl != null)
            {
                playerControl.enabled = true;
            }
        }
    }

    private void OnDestroy()
    {
        if (startButton != null)
        {
            startButton.onClick.RemoveListener(OnStartButtonPressed);
        }
    }
}