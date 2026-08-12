/*
 * Author: Cylina ho
 * Date: 12th August 2026
 * File: HomePanel.cs
 * Description:
 * Directly checks for mouse clicks over the Start Button in Update()
 * using Unity's New Input System.
 */

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem; // Required for New Input System

public class HomePanel : MonoBehaviour
{
    [Header("Home UI Setup")]
    [SerializeField] private GameObject homePanel;
    [SerializeField] private Button startButton;

    [Header("Audio Setup")]
    [SerializeField] private AudioSource backgroundMusic;

    [Header("Player Control")]
    [SerializeField] private Behaviour[] playerControlsToDisable;

    private RectTransform startButtonRect;
    private bool isStarted = false;

    private void Start()
    {
        if (startButton != null)
        {
            startButtonRect = startButton.GetComponent<RectTransform>();
        }

        if (backgroundMusic != null)
        {
            backgroundMusic.Stop();
        }

        DisablePlayerControl();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
        if (isStarted) return;

        // Detect left mouse click using New Input System
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();

            // Check if click was over the Start Button screen bounds
            if (startButtonRect != null && RectTransformUtility.RectangleContainsScreenPoint(startButtonRect, mousePosition))
            {
                ClosePageAndStart();
            }
        }
    }

    private void ClosePageAndStart()
    {
        isStarted = true;

        if (backgroundMusic != null && !backgroundMusic.isPlaying)
        {
            backgroundMusic.Play();
        }

        EnablePlayerControl();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (homePanel != null)
        {
            homePanel.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }

        Debug.Log("Start pressed: Home page closed via New Input System!");
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