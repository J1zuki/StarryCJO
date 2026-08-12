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
    [SerializeField] private GameObject homePanel;
    [SerializeField] private Button startButton;

    [Header("Audio Setup")]
    [SerializeField] private AudioSource backgroundMusic;

    [Header("Player Control")]
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
        if (backgroundMusic != null)
        {
            backgroundMusic.Stop();
        }

        DisablePlayerControl();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OnStartButtonPressed()
    {
        // 1. Play background music
        if (backgroundMusic != null && !backgroundMusic.isPlaying)
        {
            backgroundMusic.Play();
        }

        // 2. Enable player controls for gameplay
        EnablePlayerControl();

        // 3. Lock mouse cursor for FPS/3rd person control
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // 4. Turn off the home screen
        if (homePanel != null)
        {
            homePanel.SetActive(false);
        }
        else
        {
            // Fallback if homePanel wasn't assigned in inspector
            gameObject.SetActive(false);
        }

        Debug.Log("Game Started!");
    }

    private void DisablePlayerControl()
    {
        if (playerControlsToDisable == null) return;
        foreach (Behaviour c in playerControlsToDisable)
        {
            if (c != null) c.enabled = false;
        }
    }

    private void EnablePlayerControl()
    {
        if (playerControlsToDisable == null) return;
        foreach (Behaviour c in playerControlsToDisable)
        {
            if (c != null) c.enabled = true;
        }
    }
}