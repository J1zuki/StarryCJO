using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class StoreOwnerNPC : MonoBehaviour
{
    [Header("Detection Settings")]
    public float detectionRadius = 8f;
    public float stoppingDistance = 2f;
    
    [Header("References")]
    public Transform playerCapsule;
    public JaywalkingUI missionUI;

    private NavMeshAgent agent;
    private Vector3 originalPosition;
    private bool playerIsNear = false;
    private bool hasTriggeredDialogue = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = stoppingDistance;
        originalPosition = transform.position;

        // Auto-find player by tag if not manually assigned
        if (playerCapsule == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) playerCapsule = playerObj.transform;
        }
    }

    void Update()
    {
        if (playerCapsule == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerCapsule.position);

        // Detect if player is close to 7-Eleven
        if (distanceToPlayer <= detectionRadius)
        {
            playerIsNear = true;
            agent.SetDestination(playerCapsule.position);

            // Turn to face player when in stopping distance
            if (distanceToPlayer <= stoppingDistance + 0.5f)
            {
                LookAtPlayer();

                // Open dialogue UI if not already opened
                if (!hasTriggeredDialogue && missionUI != null)
                {
                    hasTriggeredDialogue = true;
                    missionUI.ShowPrompt("Hey kid! Need some quick cash? Go cross the street outside without using the crosswalk—give the local cops a hard time!", AcceptMission, DeclineMission);
                }
            }
        }
        else if (playerIsNear)
        {
            // Player left the area -> Return NPC to store
            playerIsNear = false;
            agent.SetDestination(originalPosition);
        }
    }

    private void LookAtPlayer()
    {
        Vector3 direction = (playerCapsule.position - transform.position).normalized;
        direction.y = 0; // Keep NPC upright
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 5f);
        }
    }

    private void AcceptMission()
    {
        Debug.Log("Mission Accepted: Jaywalking!");
        if (JaywalkingMission.Instance != null)
        {
            JaywalkingMission.Instance.StartMission();
        }
        missionUI.HidePrompt();
    }

    private void DeclineMission()
    {
        Debug.Log("Mission Declined.");
        missionUI.HidePrompt();
        // Allow player to trigger dialogue again later if desired
        Invoke(nameof(ResetDialogue), 5f);
    }

    private void ResetDialogue()
    {
        hasTriggeredDialogue = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}