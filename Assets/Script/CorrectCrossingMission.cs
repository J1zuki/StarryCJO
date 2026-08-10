/*
 * Author: Joyce Kwek
 * Date: 10th August 2026
 * File: CorrectCrossingMission.cs
 * Description:
 * Controls the correct crossing mission.
 * The player interacts with RightNPC to open the TryIt Canvas.
 * After pressing Okay, PlayerCapsule is moved to NextSpawnPoint
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
/// TryIt Canvas, player teleporting,
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
    /// Prepares the player references, hides the mission UI,
    /// and prepares the Okay button.
    /// </summary>
    private void Start()
    {
        tryItOpen = false;
        playerCanDemonstrate = false;
        missionCompleted = false;
        currentPoints = 0;

        if (player == null)
        {
            GameObject playerObject =
                GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
                player = playerObject.transform;
        }

        if (player != null &&
            characterController == null)
        {
            characterController =
                player.GetComponent<CharacterController>();
        }

        if (tryItCanvas != null)
            tryItCanvas.SetActive(false);

        if (completionCanvas != null)
            completionCanvas.SetActive(false);

        if (okayButton != null)
        {
            okayButton.onClick.RemoveAllListeners();

            okayButton.onClick.AddListener(
                StartPlayerDemonstration
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

        tryItCanvas.SetActive(true);

        if (tryItText != null)
            tryItText.text = tryItMessage;

        DisablePlayerControl();

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
    /// Hides the TryIt Canvas, moves PlayerCapsule
    /// to NextSpawnPoint, and begins the correct crossing.
    /// </summary>
    public void StartPlayerDemonstration()
    {
        if (!tryItOpen)
            return;

        if (tryItCanvas != null)
            tryItCanvas.SetActive(false);

        tryItOpen = false;

        TeleportPlayerToNextSpawn();

        playerCanDemonstrate = true;

        EnablePlayerControl();

        Cursor.lockState =
            CursorLockMode.Locked;

        Cursor.visible = false;

        Debug.Log(
            "Player teleported to NextSpawnPoint and can now demonstrate the correct crossing.",
            gameObject
        );
    }

    /// <summary>
    /// Moves PlayerCapsule to NextSpawnPoint.
    /// The CharacterController is temporarily disabled
    /// so the transform position can be changed safely.
    /// </summary>
    private void TeleportPlayerToNextSpawn()
    {
        if (player == null)
        {
            Debug.LogError(
                "PlayerCapsule is not assigned.",
                gameObject
            );

            return;
        }

        if (nextSpawnPoint == null)
        {
            Debug.LogError(
                "NextSpawnPoint is not assigned.",
                gameObject
            );

            return;
        }

        if (characterController != null)
            characterController.enabled = false;

        player.position =
            nextSpawnPoint.position;

        player.rotation =
            nextSpawnPoint.rotation;

        if (characterController != null)
            characterController.enabled = true;

        Debug.Log(
            "PlayerCapsule moved to NextSpawnPoint.",
            gameObject
        );
    }

    /// <summary>
    /// Completes the mission when PlayerCapsule
    /// reaches BusStopCrossPoint.
    /// </summary>
    public void CompleteCorrectCrossing()
    {
        if (!playerCanDemonstrate)
            return;

        if (missionCompleted)
            return;

        missionCompleted = true;
        playerCanDemonstrate = false;

        currentPoints += pointsAwarded;

        DisablePlayerControl();

        if (completionCanvas != null)
            completionCanvas.SetActive(true);

        if (completionTitleText != null)
            completionTitleText.text = "Well Done!";

        if (completionMessageText != null)
            completionMessageText.text = completionMessage;

        if (pointsText != null)
        {
            pointsText.text =
                "+" + pointsAwarded +
                " Points\nTotal Points: " +
                currentPoints;
        }

        Cursor.lockState =
            CursorLockMode.None;

        Cursor.visible = true;
    }

    /// <summary>
    /// Disables PlayerCapsule movement.
    /// </summary>
    private void DisablePlayerControl()
    {
        if (playerController != null)
            playerController.enabled = false;
    }

    /// <summary>
    /// Restores PlayerCapsule movement.
    /// </summary>
    private void EnablePlayerControl()
    {
        if (playerController != null)
            playerController.enabled = true;
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