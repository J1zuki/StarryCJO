/*
 * Author: Cylina ho
 * Date: 12th August 2026
 * File: HomePanel.cs
 * Description:
 * Manages the main Home Panel UI.
 * Keeps the home screen open on start, unlocks the mouse cursor,
 * and hides the home screen once the player presses the Start button.
 */

using UnityEngine;
using UnityEngine.UI;

public class HomePanel : MonoBehaviour
{
    [Header("Home UI Setup")]
    [Tooltip("Assign the Home Panel GameObject (or leave null if attached to the main UI root).")]
    [SerializeField] private GameObject homePanel;
    [SerializeField] private Button startButton;

    [Header("Audio Setup")]
    [Tooltip("Assign the AudioSource playing background music (from PlayerCapsule).")]
    [SerializeField] private AudioSource backgroundMusic;

    [Header("Player Control")]
    [Tooltip("Assign player movement scripts (e.g. StarterAssetsInputs / PlayerController) to disable while menu is open.")]
    [SerializeField] private Behaviour[] playerControlsToDisable;

    private void Awake()
    {
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

        // Show main home screen UI
        if (homePanel != null)
        {
            homePanel.SetActive(true);
        }

        // Stop music at launch
        if (backgroundMusic != null)
        {
            backgroundMusic.Stop();
        }

        DisablePlayerControl();

        // Unlock mouse cursor for UI selection
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    /// <summary>
    /// Called when Start button is clicked.
    /// Hides menu UI, starts BGM, enables player controls, and locks cursor for gameplay.
    /// </summary>
    public void OnStartButtonPressed()
    {
        // 1. Play background music
        if (backgroundMusic != null && !backgroundMusic.isPlaying)
        {
            backgroundMusic.Play();
        }

        // 2. Enable gameplay controls
        EnablePlayerControl();

        // 3. Lock cursor for First-Person / Third-Person gameplay view
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // 4. Hide Home UI Panel
        if (homePanel != null)
        {
            homePanel.SetActive(false);
        }
        else
        {
            // If homePanel slot is unassigned, deactivate this panel root
            gameObject.SetActive(false);
        }

        Debug.Log("Game Started: Menu closed, player control enabled, BGM playing.", gameObject);
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