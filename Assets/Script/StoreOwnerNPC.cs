/*
 * Author: Cylina Ho & Joyce Kwek
 * Date: 10th August 2026
 * File: StoreOwnerNPC.cs
 * Description:
 * Controls the Store Owner NPC.
 * The NPC moves toward the player using a NavMeshAgent
 * when the player enters the store interaction zone.
 * When the NPC is close enough, a Press E prompt appears.
 * Pressing E opens the Store Owner dialogue panel.
 * Pressing the Nope, Goodbye button closes the dialogue.
 */

using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// Controls the Store Owner NPC movement,
/// interaction prompt, dialogue panel,
/// dialogue button and facing direction.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class StoreOwnerNPC : MonoBehaviour
{
    [Header("NPC Movement & Distance Settings")]

    /// <summary>
    /// Movement speed of the Store Owner.
    /// </summary>
    [SerializeField] private float movementSpeed = 2f;

    /// <summary>
    /// Distance from the player where
    /// the Store Owner stops moving.
    /// </summary>
    [SerializeField] private float stoppingDistance = 2f;

    /// <summary>
    /// Maximum distance where the player
    /// can interact with the Store Owner.
    /// </summary>
    [SerializeField] private float interactDistance = 3f;

    /// <summary>
    /// Speed used when rotating
    /// the Store Owner toward the player.
    /// </summary>
    [SerializeField] private float rotationSpeed = 5f;


    [Header("Dialogue Settings")]

    /// <summary>
    /// Greeting displayed when the player
    /// talks to the Store Owner.
    /// </summary>
    [SerializeField]
    private string greetingMessage =
        "Store Owner: Hey! Welcome to 7-Eleven! How can I help you today? Or Just Looking around?";


    [Header("UI References")]

    /// <summary>
    /// UI object displaying the Press E prompt.
    /// </summary>
    [SerializeField] private GameObject interactPrompt;

    /// <summary>
    /// Panel containing the Store Owner dialogue.
    /// </summary>
    [SerializeField] private GameObject dialoguePanel;

    /// <summary>
    /// Text displaying the Store Owner dialogue.
    /// </summary>
    [SerializeField] private TMP_Text dialogueText;

    /// <summary>
    /// Button used to leave the Store Owner dialogue.
    /// </summary>
    [SerializeField] private Button leaveButton;


    [Header("Cursor Settings")]

    /// <summary>
    /// Determines whether the mouse cursor is
    /// unlocked while the dialogue is open.
    /// </summary>
    [SerializeField] private bool unlockCursorDuringDialogue = true;


    /// <summary>
    /// NavMeshAgent used to move the Store Owner.
    /// </summary>
    private NavMeshAgent agent;

    /// <summary>
    /// Transform of the player currently
    /// inside the Store Owner zone.
    /// </summary>
    private Transform playerTransform;

    /// <summary>
    /// Determines whether the player is
    /// inside the Store Owner zone.
    /// </summary>
    private bool playerIsNearStore;

    /// <summary>
    /// Determines whether the player is
    /// close enough to interact.
    /// </summary>
    private bool isPlayerInInteractRange;

    /// <summary>
    /// Determines whether the dialogue is open.
    /// </summary>
    private bool dialogueIsOpen;

    /// <summary>
    /// Prevents repeated NavMesh warning messages.
    /// </summary>
    private bool navMeshWarningShown;


    /// <summary>
    /// Gets the NavMeshAgent and prepares
    /// the Store Owner movement settings.
    /// </summary>
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.speed = movementSpeed;
        agent.stoppingDistance = stoppingDistance;
    }


    /// <summary>
    /// Prepares the Store Owner UI
    /// when the scene starts.
    /// </summary>
    private void Start()
    {
        if (interactPrompt != null)
        {
            interactPrompt.SetActive(false);
        }

        if (leaveButton != null)
        {
            leaveButton.gameObject.SetActive(false);
            leaveButton.onClick.AddListener(CloseStoreDialogue);
        }
        else
        {
            Debug.LogWarning(
                "StoreOwnerNPC: Leave Button has not been assigned."
            );
        }

        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
        else
        {
            Debug.LogWarning(
                "StoreOwnerNPC: Dialogue Panel has not been assigned."
            );
        }
    }


    /// <summary>
    /// Removes the Leave button listener
    /// when the Store Owner is destroyed.
    /// </summary>
    private void OnDestroy()
    {
        if (leaveButton != null)
        {
            leaveButton.onClick.RemoveListener(CloseStoreDialogue);
        }
    }


    /// <summary>
    /// Handles NPC movement,
    /// interaction distance,
    /// facing direction and E-key input.
    /// </summary>
    private void Update()
    {
        if (!playerIsNearStore || playerTransform == null)
        {
            return;
        }

        float distanceToPlayer = Vector3.Distance(
            transform.position,
            playerTransform.position
        );

        isPlayerInInteractRange =
            distanceToPlayer <= interactDistance;

        if (dialogueIsOpen)
        {
            StopNPC();
            LookAtPlayer();
            return;
        }

        HandleMovement(distanceToPlayer);

        UpdateInteractPrompt();

        if (isPlayerInInteractRange &&
            Keyboard.current != null &&
            Keyboard.current.eKey.wasPressedThisFrame)
        {
            OpenStoreDialogue();
        }
    }


    /// <summary>
    /// Moves the Store Owner toward the player
    /// until the stopping distance is reached.
    /// </summary>
    /// <param name="distanceToPlayer">
    /// Current distance between the Store Owner
    /// and the player.
    /// </param>
    private void HandleMovement(float distanceToPlayer)
    {
        if (!agent.isOnNavMesh)
        {
            if (!navMeshWarningShown)
            {
                Debug.LogWarning(
                    "StoreOwnerNPC: Store Owner is not on a NavMesh."
                );

                navMeshWarningShown = true;
            }

            return;
        }

        if (distanceToPlayer > stoppingDistance)
        {
            agent.isStopped = false;

            agent.SetDestination(
                playerTransform.position
            );
        }
        else
        {
            StopNPC();
            LookAtPlayer();
        }
    }


    /// <summary>
    /// Stops the Store Owner from moving
    /// and clears the current NavMesh path.
    /// </summary>
    private void StopNPC()
    {
        if (!agent.isOnNavMesh)
        {
            return;
        }

        agent.isStopped = true;

        if (agent.hasPath)
        {
            agent.ResetPath();
        }
    }


    /// <summary>
    /// Rotates the Store Owner horizontally
    /// toward the player.
    /// </summary>
    private void LookAtPlayer()
    {
        if (playerTransform == null)
        {
            return;
        }

        Vector3 direction =
            playerTransform.position - transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.001f)
        {
            return;
        }

        Quaternion targetRotation =
            Quaternion.LookRotation(direction.normalized);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }


    /// <summary>
    /// Shows the Press E prompt when the player
    /// is within interaction distance.
    /// </summary>
    private void UpdateInteractPrompt()
    {
        if (interactPrompt == null)
        {
            return;
        }

        bool shouldShowPrompt =
            isPlayerInInteractRange &&
            !dialogueIsOpen;

        interactPrompt.SetActive(shouldShowPrompt);
    }


    /// <summary>
    /// Opens the Store Owner dialogue,
    /// displays the greeting message
    /// and shows the Nope, Goodbye button.
    /// </summary>
    private void OpenStoreDialogue()
    {
        dialogueIsOpen = true;

        StopNPC();
        LookAtPlayer();

        if (interactPrompt != null)
        {
            interactPrompt.SetActive(false);
        }

        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(true);
        }
        else
        {
            Debug.LogError(
                "StoreOwnerNPC: Dialogue Panel has not been assigned."
            );

            dialogueIsOpen = false;
            return;
        }

        if (dialogueText != null)
        {
            dialogueText.text = greetingMessage;
        }
        else
        {
            Debug.LogWarning(
                "StoreOwnerNPC: Dialogue Text has not been assigned."
            );
        }

        /*
         * IMPORTANT:
         * Activates the Nope, Goodbye button
         * whenever the dialogue is opened.
         */
        if (leaveButton != null)
        {
            leaveButton.gameObject.SetActive(true);
            leaveButton.interactable = true;
        }
        else
        {
            Debug.LogWarning(
                "StoreOwnerNPC: Leave Button has not been assigned."
            );
        }

        if (unlockCursorDuringDialogue)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        Debug.Log(
            "StoreOwnerNPC: Dialogue opened."
        );
    }


    /// <summary>
    /// Closes the Store Owner dialogue
    /// when the player presses the
    /// Nope, Goodbye button.
    /// </summary>
    public void CloseStoreDialogue()
    {
        dialogueIsOpen = false;

        /*
         * Hide the button first.
         */
        if (leaveButton != null)
        {
            leaveButton.gameObject.SetActive(false);
        }

        /*
         * Hide the dialogue panel.
         */
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }

        /*
         * Lock and hide the cursor again.
         */
        if (unlockCursorDuringDialogue)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        /*
         * Show Press E again if the player
         * is still standing near the NPC.
         */
        UpdateInteractPrompt();

        Debug.Log(
            "StoreOwnerNPC: Dialogue closed."
        );
    }


    /// <summary>
    /// Called by StoreZoneTrigger when the player
    /// enters the Store Owner interaction zone.
    /// </summary>
    /// <param name="player">
    /// Transform of the player entering the zone.
    /// </param>
    public void OnPlayerEnterZone(Transform player)
    {
        if (player == null)
        {
            return;
        }

        playerTransform = player;
        playerIsNearStore = true;

        Debug.Log(
            "StoreOwnerNPC: Player entered store zone."
        );
    }


    /// <summary>
    /// Called by StoreZoneTrigger when the player
    /// exits the Store Owner interaction zone.
    /// </summary>
    public void OnPlayerExitZone()
    {
        playerIsNearStore = false;
        isPlayerInInteractRange = false;

        StopNPC();

        if (interactPrompt != null)
        {
            interactPrompt.SetActive(false);
        }

        if (dialogueIsOpen)
        {
            CloseStoreDialogue();
        }
        else
        {
            if (dialoguePanel != null)
            {
                dialoguePanel.SetActive(false);
            }

            if (leaveButton != null)
            {
                leaveButton.gameObject.SetActive(false);
            }
        }

        playerTransform = null;

        Debug.Log(
            "StoreOwnerNPC: Player exited store zone."
        );
    }
}