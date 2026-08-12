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

    [Header("Player Control")]
    [Tooltip("Assign player control scripts here to freeze the player while the Home Panel is active.")]
    [SerializeField] private Behaviour[] playerControlsToDisable;

    /// <summary>
    /// Configures start button events, shows the Home Panel,
    /// and pauses player inputs when the scene begins.
    /// </summary>
    private void Start()
    {
        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartButtonPressed);
        }
        else
        {
            Debug.LogWarning("HomePanelController: Start Button has not been assigned.", gameObject);
        }

        // Show home panel initially
        if (homePanel != null)
        {
            homePanel.SetActive(true);
        }

        DisablePlayerControl();

        // Unlock mouse cursor for UI navigation
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    /// <summary>
    /// Called when the player clicks the Start button.
    /// Hides the Home Panel, enables gameplay controls, and locks the cursor.
    /// </summary>
    public void OnStartButtonPressed()
    {
        // Hide the Home Panel
        if (homePanel != null)
        {
            homePanel.SetActive(false);
        }

        EnablePlayerControl();

        // Lock and hide cursor for gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log("Game Started: Home Panel closed.", gameObject);
    }

    /// <summary>
    /// Disables player movement scripts while on the home panel.
    /// </summary>
    private void DisablePlayerControl()
    {
        if (playerControlsToDisable == null)
        {
            return;
        }

        foreach (Behaviour playerControl in playerControlsToDisable)
        {
            if (playerControl != null)
            {
                playerControl.enabled = false;
            }
        }
    }

    /// <summary>
    /// Enables player movement scripts once the game starts.
    /// </summary>
    private void EnablePlayerControl()
    {
        if (playerControlsToDisable == null)
        {
            return;
        }

        foreach (Behaviour playerControl in playerControlsToDisable)
        {
            if (playerControl != null)
            {
                playerControl.enabled = true;
            }
        }
    }

    /// <summary>
    /// Removes button listeners when this script is destroyed.
    /// </summary>
    private void OnDestroy()
    {
        if (startButton != null)
        {
            startButton.onClick.RemoveListener(OnStartButtonPressed);
        }
    }
}