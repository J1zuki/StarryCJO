using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class StoreOwnerNPC : MonoBehaviour
{
    [Header("NPC Movement & Distance Settings")]
    [SerializeField] private float stoppingDistance = 2.0f;
    [SerializeField] private float interactDistance = 3.0f;
    [SerializeField] private string greetingMessage = "Welcome to 7-Eleven! What can I get for you?";

    [Header("Keybindings")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    private NavMeshAgent agent;
    private Transform playerTransform;
    private bool playerIsNearStore = false;
    private bool isPlayerInInteractRange = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = stoppingDistance;
    }

    private void Update()
    {
        if (playerIsNearStore && playerTransform != null)
        {
            // Move NavMeshAgent toward player
            agent.SetDestination(playerTransform.position);

            // Check distance between NPC and Player
            float distToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            isPlayerInInteractRange = distToPlayer <= interactDistance;

            // Face the player when stopped
            if (distToPlayer <= stoppingDistance)
            {
                LookAtPlayer();
            }

            // Interact trigger
            if (isPlayerInInteractRange && Input.GetKeyDown(interactKey))
            {
                DialogueUI.Instance.OpenDialogue(greetingMessage);
            }
        }
    }

    private void LookAtPlayer()
    {
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        direction.y = 0; // Keep rotation upright
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    // Triggered by the Store Zone script or direct trigger detection
    public void OnPlayerEnterZone(Transform player)
    {
        playerTransform = player;
        playerIsNearStore = true;
    }

    public void OnPlayerExitZone()
    {
        playerIsNearStore = false;
        playerTransform = null;
        if (agent.isOnNavMesh)
        {
            agent.ResetPath();
        }
        DialogueUI.Instance.CloseDialogue();
    }
}