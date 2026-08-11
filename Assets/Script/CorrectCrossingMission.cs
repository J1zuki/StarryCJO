/*
 * Author: Joyce Kwek
 * Date: 10th August 2026
 * File: CorrectCrossingMission.cs
 * Description:
 * Controls the correct crossing mission.
 * The player interacts with RightNPC to open the TryIt Canvas.
 * After pressing Okay, PlayerCapsule is moved safely to NextSpawnPoint
 * before starting the correct road-crossing demonstration.
 * Reaching BusStopCrossPoint completes the mission
 * and awards points.
 */

using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the correct crossing mission,
/// including RightNPC interaction,
/// TryIt Canvas, safe player teleporting,
/// final feedback, and points.
/// </summary>
public class CorrectCrossingMission : MonoBehaviour
{
    [Header("Right NPC")]
    [SerializeField] private RightNPCInteraction rightNPC;

    [Header("Player")]
    [SerializeField] private Transform player;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Behaviour playerController;
    [SerializeField] private Transform nextSpawnPoint;

    [Header("Try It Canvas")]
    [SerializeField] private GameObject tryItCanvas;
    [SerializeField] private TMP_Text tryItText;
    [SerializeField] private Button okayButton;

    [TextArea(2, 4)]
    [SerializeField]
    private string tryItMessage =
        "Now let me see you show the right way.";

    [Header("Completion UI")]
    [SerializeField] private GameObject completionCanvas;
    [SerializeField] private TMP_Text completionTitleText;
    [SerializeField] private TMP_Text completionMessageText;
    [SerializeField] private TMP_Text pointsText;

    [TextArea(2, 4)]
    [SerializeField]
    private string completionMessage =
        "Well done! You demonstrated the correct way to cross the road. Keep it up!";

    [Header("Points")]
    [SerializeField] private int pointsAwarded = 100;

    private int currentPoints;

    private bool tryItOpen;
    private bool playerCanDemonstrate;
    private bool missionCompleted;

    /// <summary>
    /// Returns whether the player can currently
    /// perform the correct crossing demonstration.
    /// </summary>
    public bool PlayerCanDemonstrate =>
        playerCanDemonstrate;

    /// <summary>
    /// Returns whether the mission has been completed.
    /// </summary>
    public bool MissionCompleted =>
        missionCompleted;

    /// <summary>
    /// Finds missing player references, hides the mission UI,
    /// and prepares the Okay button when the scene begins.
    /// </summary>
    private void Start()
    {
        tryItOpen = false;
        playerCanDemonstrate = false;
        missionCompleted = false;
        currentPoints = 0;

        FindPlayerReferences();

        if (tryItCanvas != null)
        {
            tryItCanvas.SetActive(false);
        }
        else
        {
            Debug.LogError(
                "TryIt Canvas is not assigned.",
                gameObject
            );
        }

        if (completionCanvas != null)
        {
            completionCanvas.SetActive(false);
        }

        if (okayButton != null)
        {
            okayButton.onClick.RemoveListener(
                StartPlayerDemonstration
            );

            okayButton.onClick.AddListener(
                StartPlayerDemonstration
            );
        }
        else
        {
            Debug.LogError(
                "Okay Button is not assigned.",
                gameObject
            );
        }
    }

    /// <summary>
    /// Finds PlayerCapsule and its CharacterController
    /// automatically when references are missing.
    /// </summary>
    private void FindPlayerReferences()
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

                return;
            }
        }

        if (characterController == null)
        {
            characterController =
                player.GetComponent<CharacterController>();
        }

        if (characterController == null)
        {
            Debug.LogWarning(
                "CharacterController was not found on PlayerCapsule.",
                gameObject
            );
        }
    }

    /// <summary>
    /// Opens the TryIt Canvas after the player
    /// presses E beside RightNPC.
    /// </summary>
    public void ShowTryItCanvas()
    {
        if (tryItOpen)
            return;

        if (tryItCanvas == null)
        {
            Debug.LogError(
                "TryIt Canvas is not assigned.",
                gameObject
            );

            return;
        }

        tryItOpen = true;

        DisablePlayerControl();

        tryItCanvas.SetActive(true);

        if (tryItText != null)
        {
            tryItText.text =
                tryItMessage;
        }

        Cursor.lockState =
            CursorLockMode.None;

        Cursor.visible = true;

        Debug.Log(
            "TryIt Canvas opened.",
            gameObject
        );
    }

    /// <summary>
    /// Handles the Okay button.
    /// Hides the TryIt Canvas, safely moves PlayerCapsule
    /// to NextSpawnPoint, and begins the correct crossing.
    /// </summary>
    public void StartPlayerDemonstration()
    {
        if (!tryItOpen)
            return;

        if (tryItCanvas != null)
        {
            tryItCanvas.SetActive(false);
        }

        tryItOpen = false;

        bool teleportSucceeded =
            TeleportPlayerToNextSpawn();

        if (!teleportSucceeded)
        {
            Debug.LogError(
                "Correct crossing could not start because the player teleport failed.",
                gameObject
            );

            EnablePlayerControl();

            Cursor.lockState =
                CursorLockMode.Locked;

            Cursor.visible = false;

            return;
        }

        playerCanDemonstrate = true;

        EnablePlayerControl();

        Cursor.lockState =
            CursorLockMode.Locked;

        Cursor.visible = false;

        Debug.Log(
            "PlayerCapsule teleported to NextSpawnPoint " +
            "and can now demonstrate the correct crossing.",
            gameObject
        );
    }

    /// <summary>
    /// Safely teleports PlayerCapsule to NextSpawnPoint.
    /// The movement script is disabled before the
    /// CharacterController is disabled.
    /// </summary>
    /// <returns>
    /// True if the player was successfully moved.
    /// </returns>
    private bool TeleportPlayerToNextSpawn()
    {
        if (player == null)
        {
            Debug.LogError(
                "PlayerCapsule is not assigned.",
                gameObject
            );

            return false;
        }

        if (nextSpawnPoint == null)
        {
            Debug.LogError(
                "NextSpawnPoint is not assigned.",
                gameObject
            );

            return false;
        }

        /*
         * IMPORTANT ORDER:
         *
         * 1. Disable ThirdPersonController.
         * 2. Disable CharacterController.
         * 3. Move the player.
         * 4. Enable CharacterController.
         * 5. ThirdPersonController is enabled later.
         */

        if (playerController != null)
        {
            playerController.enabled = false;
        }

        if (characterController != null)
        {
            characterController.enabled = false;
        }

        player.SetPositionAndRotation(
            nextSpawnPoint.position,
            nextSpawnPoint.rotation
        );

        if (characterController != null)
        {
            characterController.enabled = true;
        }

        Debug.Log(
            "PlayerCapsule safely moved to NextSpawnPoint.",
            gameObject
        );

        return true;
    }

    /// <summary>
    /// Completes the mission when PlayerCapsule
    /// reaches BusStopCrossPoint.
    /// </summary>
    public void CompleteCorrectCrossing()
    {
        if (!playerCanDemonstrate)
        {
            Debug.LogWarning(
                "BusStopCrossPoint was reached before " +
                "the correct crossing demonstration started.",
                gameObject
            );

            return;
        }

        if (missionCompleted)
            return;

        missionCompleted = true;
        playerCanDemonstrate = false;

        currentPoints += pointsAwarded;

        DisablePlayerControl();

        if (completionCanvas != null)
        {
            completionCanvas.SetActive(true);
        }
        else
        {
            Debug.LogError(
                "Completion Canvas is not assigned.",
                gameObject
            );
        }

        if (completionTitleText != null)
        {
            completionTitleText.text =
                "Well Done!";
        }

        if (completionMessageText != null)
        {
            completionMessageText.text =
                completionMessage;
        }

        if (pointsText != null)
        {
            pointsText.text =
                "+" + pointsAwarded +
                " Points\n" +
                "Total Points: " +
                currentPoints;
        }

        Cursor.lockState =
            CursorLockMode.None;

        Cursor.visible = true;

        Debug.Log(
            "Correct crossing completed. Player received " +
            pointsAwarded + " points.",
            gameObject
        );
    }

    /// <summary>
    /// Disables PlayerCapsule movement.
    /// </summary>
    private void DisablePlayerControl()
    {
        if (playerController != null)
        {
            playerController.enabled = false;
        }
    }

    /// <summary>
    /// Restores PlayerCapsule movement.
    /// </summary>
    private void EnablePlayerControl()
    {
        if (playerController != null)
        {
            playerController.enabled = true;
        }
    }

    /// <summary>
    /// Removes the Okay button listener when
    /// this component is destroyed.
    /// </summary>
    private void OnDestroy()
    {
        if (okayButton != null)
        {
            okayButton.onClick.RemoveListener(
                StartPlayerDemonstration
            );
        }
    }
}