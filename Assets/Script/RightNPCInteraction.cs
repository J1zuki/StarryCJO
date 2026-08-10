/*
 * Author: Joyce Kwek
 * Date: 10th August 2026
 * File: RightNPCInteraction.cs
 * Description:
 * Allows PlayerCapsule to interact with RightNPC.
 * When the player is close enough and presses E,
 * the TryIt Canvas opens immediately.
 */

using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controls interaction between PlayerCapsule
/// and RightNPC.
/// </summary>
public class RightNPCInteraction : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private CorrectCrossingMission missionManager;

    [Header("Interaction Prompt")]
    [SerializeField] private GameObject interactionPrompt;
    [SerializeField] private TMP_Text interactionPromptText;

    [Header("Interaction")]
    [SerializeField] private float interactionDistance = 4f;

    private bool interactionUsed;

    /// <summary>
    /// Finds PlayerCapsule automatically if necessary
    /// and hides the prompt when the scene begins.
    /// </summary>
    private void Start()
    {
        if (player == null)
        {
            GameObject playerObject =
                GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                player = playerObject.transform;
            }
            else
            {
                Debug.LogError(
                    "PlayerCapsule could not be found. " +
                    "Make sure it has the Player tag.",
                    gameObject
                );
            }
        }

        if (missionManager == null)
        {
            Debug.LogError(
                "CorrectCrossingMission is not assigned to RightNPC.",
                gameObject
            );
        }

        HidePrompt();
    }

    /// <summary>
    /// Checks whether PlayerCapsule is close enough
    /// to RightNPC and listens for the E key.
    /// </summary>
    private void Update()
    {
        if (interactionUsed)
        {
            HidePrompt();
            return;
        }

        if (player == null ||
            missionManager == null)
        {
            HidePrompt();
            return;
        }

        float distance =
            Vector3.Distance(
                transform.position,
                player.position
            );

        if (distance > interactionDistance)
        {
            HidePrompt();
            return;
        }

        ShowPrompt();

        if (Keyboard.current == null)
            return;

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            InteractWithRightNPC();
        }
    }

    /// <summary>
    /// Displays the RightNPC interaction prompt.
    /// </summary>
    private void ShowPrompt()
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(true);
        }

        if (interactionPromptText != null)
        {
            interactionPromptText.text =
                "Press 'E' to talk to RightNPC";
        }
    }

    /// <summary>
    /// Hides the RightNPC interaction prompt.
    /// </summary>
    private void HidePrompt()
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }
    }

    /// <summary>
    /// Opens the TryIt Canvas immediately
    /// when the player presses E.
    /// </summary>
    private void InteractWithRightNPC()
    {
        if (interactionUsed)
            return;

        interactionUsed = true;

        HidePrompt();

        Debug.Log(
            "E pressed on RightNPC. Opening TryIt Canvas.",
            gameObject
        );

        missionManager.ShowTryItCanvas();
    }
}