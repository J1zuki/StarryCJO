using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PoliceOfficer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform officerStopPoint;

    [Header("Movement")]
    [SerializeField] private float stoppingDistance = 0.3f;
    [SerializeField] private float stopPointSearchRadius = 2f;

    [Header("Debug")]
    [SerializeField] private bool drawPath = true;

    private Animator officerAnimator;
    private NavMeshPath calculatedPath;

    private bool activated;
    private bool stopped;

    private Vector3 destination;

    private void Awake()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        officerAnimator = GetComponentInChildren<Animator>();
        calculatedPath = new NavMeshPath();

        if (officerAnimator != null)
            officerAnimator.applyRootMotion = false;

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
                "OfficerStopPoint has not been assigned.",
                gameObject
            );

            return false;
        }

        if (!agent.isOnNavMesh)
        {
            Debug.LogError(
                "Police officer is not standing on a baked NavMesh.",
                gameObject
            );

            return false;
        }

        NavMeshHit stopPointHit;

        bool stopPointFound = NavMesh.SamplePosition(
            officerStopPoint.position,
            out stopPointHit,
            stopPointSearchRadius,
            agent.areaMask
        );

        if (!stopPointFound)
        {
            Debug.LogError(
                "No NavMesh was found near OfficerStopPoint. " +
                "Move the stop point onto the cyan NavMesh.",
                officerStopPoint.gameObject
            );

            return false;
        }

        destination = stopPointHit.position;

        calculatedPath.ClearCorners();

        bool pathFound = agent.CalculatePath(
            destination,
            calculatedPath
        );

        if (!pathFound)
        {
            Debug.LogError(
                "Unity could not calculate a path to OfficerStopPoint.",
                gameObject
            );

            return false;
        }

        if (calculatedPath.status != NavMeshPathStatus.PathComplete)
        {
            Debug.LogError(
                "The path to OfficerStopPoint is not complete. " +
                "The officer's NavMesh and the destination NavMesh " +
                "are probably disconnected. Path status: " +
                calculatedPath.status,
                gameObject
            );

            DrawCalculatedPath(Color.red);

            return false;
        }

        activated = true;
        stopped = false;

        agent.ResetPath();
        agent.isStopped = false;
        agent.SetPath(calculatedPath);

        Debug.Log(
            "Officer moving from " +
            agent.transform.position +
            " to " +
            destination
        );

        DrawCalculatedPath(Color.green);

        return true;
    }

    private void Update()
    {
        UpdateAnimation();

        if (!activated || stopped)
            return;

        if (!agent.isOnNavMesh)
            return;

        if (agent.pathPending)
            return;

        if (agent.pathStatus != NavMeshPathStatus.PathComplete)
        {
            Debug.LogError(
                "Officer lost the complete path. Current status: " +
                agent.pathStatus,
                gameObject
            );

            StopOfficer();
            return;
        }

        if (float.IsInfinity(agent.remainingDistance))
            return;

        bool reachedDestination =
            agent.remainingDistance <= agent.stoppingDistance &&
            (!agent.hasPath || agent.velocity.sqrMagnitude <= 0.01f);

        if (reachedDestination)
            StopOfficer();
    }

    private void StopOfficer()
    {
        if (stopped)
            return;

        stopped = true;
        activated = false;

        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        agent.ResetPath();

        NavMeshHit finalPosition;

        if (NavMesh.SamplePosition(
            destination,
            out finalPosition,
            stopPointSearchRadius,
            agent.areaMask))
        {
            agent.Warp(finalPosition.position);
        }

        UpdateAnimation();

        Debug.Log(
            "Officer stopped at: " +
            transform.position
        );
    }

    private void UpdateAnimation()
    {
        if (officerAnimator == null)
            return;

        float movementSpeed = agent.velocity.magnitude;

        if (HasAnimatorParameter("Speed"))
            officerAnimator.SetFloat("Speed", movementSpeed);
    }

    private void DrawCalculatedPath(Color colour)
    {
        if (!drawPath || calculatedPath == null)
            return;

        Vector3[] corners = calculatedPath.corners;

        for (int i = 0; i < corners.Length - 1; i++)
        {
            Debug.DrawLine(
                corners[i],
                corners[i + 1],
                colour,
                10f
            );
        }
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

    private void OnDrawGizmosSelected()
    {
        if (officerStopPoint == null)
            return;

        Gizmos.DrawWireSphere(
            officerStopPoint.position,
            stopPointSearchRadius
        );

        Gizmos.DrawLine(
            transform.position,
            officerStopPoint.position
        );
    }
}