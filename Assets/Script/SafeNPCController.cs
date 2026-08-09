using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class SafeNPCController : MonoBehaviour
{
    [Header("Movement Points")]
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform targetPoint;

    [Header("Dialogue & Settings")]
    [SerializeField] private TMP_Text miniDialogue;
    [SerializeField] private float walkSpeed = 2.5f;

    private NavMeshAgent agent;
    private bool hasCrossed = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = walkSpeed;
    }

    private void Start()
    {
        if (startPoint != null) agent.Warp(startPoint.position);
    }

    public void StartSafeCrossing()
    {
        if (hasCrossed) return;
        hasCrossed = true;
        StartCoroutine(RunSafeCrossingSequence());
    }

    private IEnumerator RunSafeCrossingSequence()
    {
        if (miniDialogue != null)
        {
            miniDialogue.gameObject.SetActive(true);
            miniDialogue.text = "Light is green! Looking left and right before crossing...";
        }

        // Look Left & Right animation delays
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y - 45f, 0);
        yield return new WaitForSeconds(0.8f);

        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y + 90f, 0);
        yield return new WaitForSeconds(0.8f);

        if (miniDialogue != null) miniDialogue.text = "Crossing safely!";

        // Use NavMeshAgent to move to target point
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