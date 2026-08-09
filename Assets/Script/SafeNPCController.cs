using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class SafeNPCController : MonoBehaviour
{
    [Header("Movement Points")]
    [SerializeField] private Transform targetPoint;
    [SerializeField] private Transform playerTransform;

    [Header("Dialogue & UI")]
    [SerializeField] private TMP_Text miniDialogue;
    [SerializeField] private float walkSpeed = 2.5f;
    [SerializeField] private float followDistance = 2f;

    private NavMeshAgent agent;
    private bool isFollowingPlayer = false;
    private bool hasCrossed = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = walkSpeed;
    }

    private void Update()
    {
        // Follow player continuously if dialogue action was accepted
        if (isFollowingPlayer && !hasCrossed && playerTransform != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            if (distanceToPlayer > followDistance)
            {
                agent.SetDestination(playerTransform.position);
            }
            else
            {
                agent.ResetPath(); // Stop moving when close enough to player
            }
        }
    }

    public void OnPlayerTalk()
    {
        if (miniDialogue != null)
        {
            miniDialogue.gameObject.SetActive(true);
            miniDialogue.text = "How to cross? Can you show me the way?";
        }

        // Start following the player
        if (playerTransform == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) playerTransform = playerObj.transform;
        }

        isFollowingPlayer = true;
    }

    public void StartSafeCrossing()
    {
        if (hasCrossed) return;
        hasCrossed = true;
        isFollowingPlayer = false; // Stop following player, focus on target crosswalk point
        StartCoroutine(RunSafeCrossingSequence());
    }

    private IEnumerator RunSafeCrossingSequence()
    {
        if (miniDialogue != null)
        {
            miniDialogue.gameObject.SetActive(true);
            miniDialogue.text = "Light is green! Looking left and right before crossing...";
        }

        // Look Left & Right check
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y - 45f, 0);
        yield return new WaitForSeconds(0.8f);

        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y + 90f, 0);
        yield return new WaitForSeconds(0.8f);

        if (miniDialogue != null) miniDialogue.text = "Crossing safely!";

        if (targetPoint != null)
        {
            agent.SetDestination(targetPoint.position);

            while (agent.pathPending || agent.remainingDistance > 0.5f)
            {
                yield return null;
            }
        }

        yield return new WaitForSeconds(1.5f);
        if (miniDialogue != null) miniDialogue.gameObject.SetActive(false);
    }
}