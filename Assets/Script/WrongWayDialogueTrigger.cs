/*
 * Author: Joyce Kwek
 * Date: 10th August 2026
 * File: WrongWayDialogueTrigger.cs
 * Description:
 * Detects when the player reaches the traffic light.
 * It stops the player, displays an observation dialogue,
 * starts NPCGirl crossing on red, and restores player movement
 * after the demonstration.
 */

using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// Starts NPCGirl's unsafe crossing demonstration when
/// the player reaches the traffic light observation area.
/// </summary>
public class WrongWayDialogueTrigger : MonoBehaviour
{
    [Header("NPC Girl")]
    [SerializeField] private NPCGirl npcGirl;

    [Header("Dialogue")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;

    [TextArea(2, 5)]
    [SerializeField]
    private string warningDialogue =
        "Look at the pedestrian ahead. " +
        "She is crossing even though the pedestrian signal is red.";

    [Header("Timing")]
    [SerializeField] private float startDelay = 0.5f;
    [SerializeField] private float minimumDialogueTime = 3f;

    [Header("Player Control")]
    [SerializeField] private Behaviour playerController;

    private bool hasTriggered;

    /// <summary>
    /// Hides the dialogue panel when the scene begins.
    /// </summary>
    private void Start()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }

    /// <summary>
    /// Detects when PlayerCapsule enters the observation trigger.
    /// </summary>
    /// <param name="other">
    /// Collider that entered the trigger.
    /// </param>
    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered)
            return;

        if (!other.CompareTag("Player"))
            return;

        hasTriggered = true;

        Debug.Log(
            "Player reached WrongWayDialogue trigger.",
            gameObject
        );

        StartCoroutine(
            WrongWaySequence()
        );
    }

    /// <summary>
    /// Stops the player, displays the warning,
    /// starts NPCGirl crossing on red, waits for NPCGirl
    /// to finish, and then restores player movement.
    /// </summary>
    private IEnumerator WrongWaySequence()
    {
        DisablePlayer();

        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);

        if (dialogueText != null)
            dialogueText.text = warningDialogue;

        yield return new WaitForSeconds(
            startDelay
        );

        if (npcGirl != null)
        {
            npcGirl.StartWrongCrossing();
        }
        else
        {
            Debug.LogError(
                "NPCGirl has not been assigned.",
                gameObject
            );
        }

        float dialogueTimer = 0f;

        while (npcGirl != null &&
               !npcGirl.HasCrossed)
        {
            dialogueTimer += Time.deltaTime;
            yield return null;
        }

        if (dialogueTimer < minimumDialogueTime)
        {
            yield return new WaitForSeconds(
                minimumDialogueTime - dialogueTimer
            );
        }

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        EnablePlayer();

        Collider triggerCollider =
            GetComponent<Collider>();

        if (triggerCollider != null)
            triggerCollider.enabled = false;
    }

    /// <summary>
    /// Disables PlayerCapsule movement while the
    /// unsafe demonstration is taking place.
    /// </summary>
    private void DisablePlayer()
    {
        if (playerController != null)
            playerController.enabled = false;
    }

    /// <summary>
    /// Restores PlayerCapsule movement after
    /// the demonstration finishes.
    /// </summary>
    private void EnablePlayer()
    {
        if (playerController != null)
            playerController.enabled = true;
    }
}