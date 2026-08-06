using UnityEngine;
using UnityEngine.AI;

public class PoliceOfficer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform officerStopPoint;

    [Header("Movement")]
    [SerializeField] private float stoppingDistance = 0.3f;
    [SerializeField] private float searchRadius = 0.5f;

    private Animator officerAnimator;

    private bool activated;
    private bool stopped;
    private bool pathCalculated;

    private Vector3 destination;

    private void Awake()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        officerAnimator = GetComponentInChildren<Animator>();

        // Prevent the walking animation from physically moving the officer.
        if (officerAnimator != null)
            officerAnimator.applyRootMotion = false;

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent is missing.", gameObject);
            enabled = false;
            return;
        }

        agent.updatePosition = true;
        agent.updateRotation = true;
        agent.autoBraking = true;
        agent.stoppingDistance = stoppingDistance;
        agent.isStopped = true;
    }

    public bool InterceptPlayer()
    {
        if (activated)
            return false;

        if (officerStopPoint == null)
        {
            Debug.LogError(
                "OfficerStopPoint is not assigned.",
                gameObject
            );

            return false;
        }

        if (!agent.isOnNavMesh)
        {
            Debug.LogError(
                "The officer is not standing on the NavMesh.",
                gameObject
            );

            return false;
        }

        NavMeshHit hit;

        if (!NavMesh.SamplePosition(
            officerStopPoint.position,
            out hit,
            searchRadius,
            agent.areaMask))
        {
            Debug.LogError(
                "OfficerStopPoint is not on the NavMesh.",
                officerStopPoint.gameObject
            );

            return false;
        }

        destination = hit.position;

        activated = true;
        stopped = false;
        pathCalculated = false;

        agent.ResetPath();
        agent.isStopped = false;
        agent.velocity = Vector3.zero;

        if (!agent.SetDestination(destination))
        {
            activated = false;
            agent.isStopped = true;

            Debug.LogError(
                "The officer could not create a path.",
                gameObject
            );

            return false;
        }

        Debug.Log("Officer position: " + transform.position);
        Debug.Log("Officer destination: " + destination);

        return true;
    }

    private void Update()
    {
        if (!activated || stopped)
            return;

        if (!agent.isOnNavMesh)
            return;

        if (agent.pathPending)
            return;

        if (agent.pathStatus == NavMeshPathStatus.PathInvalid)
        {
            Debug.LogError("Officer path is invalid.", gameObject);
            StopOfficer();
            return;
        }

        pathCalculated = true;

        if (!pathCalculated)
            return;

        float distance = Vector3.Distance(
            agent.nextPosition,
            destination
        );

        bool reachedDestination =
            agent.remainingDistance <= stoppingDistance ||
            distance <= stoppingDistance;

        if (reachedDestination)
            StopOfficer();
    }

    private void StopOfficer()
    {
        if (stopped)
            return;

        stopped = true;

        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        agent.ResetPath();

        // Lock the officer exactly onto the stop point.
        NavMeshHit hit;

        if (NavMesh.SamplePosition(
            destination,
            out hit,
            searchRadius,
            agent.areaMask))
        {
            agent.Warp(hit.position);
            transform.position = hit.position;
        }

        if (officerAnimator != null)
        {
            officerAnimator.applyRootMotion = false;

            // Replace "Speed" if your Animator uses another parameter.
            if (HasAnimatorParameter("Speed"))
                officerAnimator.SetFloat("Speed", 0f);
        }

        Debug.Log("Officer stopped at: " + transform.position);
    }

    private bool HasAnimatorParameter(string parameterName)
    {
        if (officerAnimator == null)
            return false;

        foreach (AnimatorControllerParameter parameter
                 in officerAnimator.parameters)
        {
            if (parameter.name == parameterName)
                return true;
        }

        return false;
    }
}