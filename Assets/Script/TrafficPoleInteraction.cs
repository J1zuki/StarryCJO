/*
 * Author: Joyce Kwek
 * Date: 11th August 2026
 * File: TrafficPoleInteraction.cs
 * Description:
 * Detects when the player is close to the traffic pole.
 * Displays the B interaction prompt and only allows the
 * traffic light button to be activated while the player
 * is inside the traffic pole interaction trigger.
 */

using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controls player interaction with the
/// pedestrian traffic-light pole.
/// </summary>
public class TrafficPoleInteraction : MonoBehaviour
{
    [Header("Traffic Light")]
    [SerializeField]
    private TrafficLightControl trafficLight;

    [Header("Interaction UI")]
    [SerializeField]
    private GameObject promptPanel;

    [SerializeField]
    private TMP_Text promptText;

    [TextArea(1, 3)]
    [SerializeField]
    private string promptMessage =
        "Press 'B' to interact with the traffic light.";

    [Header("Player")]
    [SerializeField]
    private string playerTag = "Player";

    private bool playerInside;

    /// <summary>
    /// Hides the interaction prompt
    /// when the game begins.
    /// </summary>
    private void Start()
    {
        HidePrompt();
    }

    /// <summary>
    /// Checks for B only while the player
    /// is inside the traffic pole trigger.
    /// </summary>
    private void Update()
    {
        if (!playerInside)
        {
            return;
        }

        if (Keyboard.current == null)
        {
            return;
        }

        if (Keyboard.current.bKey.wasPressedThisFrame)
        {
            PressTrafficButton();
        }
    }

    /// <summary>
    /// Detects the player entering the
    /// traffic pole interaction area.
    /// </summary>
    private void OnTriggerEnter(
        Collider other)
    {
        if (!other.CompareTag(playerTag))
        {
            return;
        }

        playerInside = true;

        ShowPrompt();

        Debug.Log(
            "Player entered traffic pole interaction area.",
            gameObject
        );
    }

    /// <summary>
    /// Detects the player leaving the
    /// traffic pole interaction area.
    /// </summary>
    private void OnTriggerExit(
        Collider other)
    {
        if (!other.CompareTag(playerTag))
        {
            return;
        }

        playerInside = false;

        HidePrompt();

        Debug.Log(
            "Player left traffic pole interaction area.",
            gameObject
        );
    }

    /// <summary>
    /// Activates the pedestrian crossing button.
    /// </summary>
    private void PressTrafficButton()
    {
        if (trafficLight == null)
        {
            Debug.LogWarning(
                "TrafficLightControl has not been assigned.",
                gameObject
            );

            return;
        }

        trafficLight.InteractWithButton();

        HidePrompt();

        Debug.Log(
            "Player pressed B at the traffic pole.",
            gameObject
        );
    }

    /// <summary>
    /// Shows the B interaction prompt.
    /// </summary>
    private void ShowPrompt()
    {
        if (promptText != null)
        {
            promptText.text =
                promptMessage;
        }

        if (promptPanel != null)
        {
            promptPanel.SetActive(true);
        }
    }

    /// <summary>
    /// Hides the B interaction prompt.
    /// </summary>
    private void HidePrompt()
    {
        if (promptPanel != null)
        {
            promptPanel.SetActive(false);
        }
    }
}