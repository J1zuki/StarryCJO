/*
 * Author: Joyce Kwek
 * Date: 10th August 2026
 * File: CorrectCrossingMission.cs
 * Description:
 * Controls the correct crossing mission.
 * The player interacts with RightNPC to open the TryIt Canvas.
 * After pressing Okay, the player can demonstrate
 * the correct road-crossing method.
 * Reaching BusStopCrossPoint completes the mission
 * and displays the final result with points.
 */

using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the correct crossing mission,
/// including RightNPC interaction,
/// TryIt Canvas, player demonstration,
/// final feedback, and points.
/// </summary>
public class CorrectCrossingMission : MonoBehaviour
{
    [Header("Right NPC")]
    [SerializeField] private RightNPCInteraction rightNPC;

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

    [Header("Player Control")]
    [SerializeField] private Behaviour playerController;

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
    /// Returns whether the correct crossing mission
    /// has already been completed.
    /// </summary>
    public bool MissionCompleted =>
        missionCompleted;

    /// <summary>
    /// Hides the TryIt and completion Canvas objects
    /// and prepares the Okay button when the scene begins.
    /// </summary>
    private void Start()
    {
        tryItOpen = false;
        playerCanDemonstrate = false;
        missionCompleted = false;
        currentPoints = 0;

        if (tryItCanvas != null)
        {
            tryItCanvas.SetActive(false);
        }
        else
        {
            Debug.LogError(
                "TryIt Canvas has not been assigned.",
                gameObject
            );
        }

        if (completionCanvas != null)
        {
            completionCanvas.SetActive(false);
        }

        if (okayButton != null)
        {
            okayButton.onClick.RemoveAllListeners();
            okayButton.onClick.AddListener(
                StartPlayerDemonstration
            );
        }
        else
        {
            Debug.LogError(
                "Okay Button has not been assigned.",
                gameObject
            );
        }
    }

    /// <summary>
    /// Opens the TryIt Canvas immediately after
    /// the player presses E beside RightNPC.
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
        {
            tryItText.text =
                tryItMessage;
        }

        DisablePlayerControl();

        Cursor.lockState =
            CursorLockMode.None;

        Cursor.visible = true;

        Debug.Log(
            "TryIt Canvas opened after interacting with RightNPC.",
            gameObject
        );
    }

    /// <summary>
    /// Handles the Okay button on the TryIt Canvas.
    /// It hides the Canvas and allows the player
    /// to demonstrate the correct crossing method.
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
        playerCanDemonstrate = true;

        EnablePlayerControl();

        Cursor.lockState =
            CursorLockMode.Locked;

        Cursor.visible = false;

        Debug.Log(
            "TryIt Canvas closed. Player can now demonstrate the correct crossing.",
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
        {
            Debug.LogWarning(
                "Player reached BusStopCrossPoint before starting the correct crossing demonstration.",
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
                "Completion Canvas has not been assigned.",
                gameObject
            );

            return;
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
    /// Disables PlayerCapsule movement while
    /// a mission Canvas is open.
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
    /// Removes the Okay button listener
    /// when this component is destroyed.
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