using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PoliceOfficer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform officerStopPoint;
    [SerializeField] private Transform player;

    [Header("Movement")]
    [SerializeField] private float stoppingDistance = 0.3f;
    [SerializeField] private float stopPointSearchRadius = 2f;

    [Header("Officer Rotation")]
    [SerializeField] private bool facePlayerAfterStopping = true;
    [SerializeField] private float rotationSpeed = 5f;

    private Animator officerAnimator;
    private NavMeshPath calculatedPath;

    private bool activated;
    private bool stopped;
    private bool canInteract;

    private Vector3 destination;

    public bool CanInteract => canInteract;
    public bool HasStopped => stopped;

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

    private void Update()
    {
        UpdateAnimation();

        if (stopped)
        {
            FacePlayer();
            return;
        }

        if (!activated)
            return;

        if (!agent.isOnNavMesh)
            return;

        if (agent.pathPending)
            return;

        if (agent.pathStatus != NavMeshPathStatus.PathComplete)
        {
            Debug.LogError(
                "Officer path is no longer complete.",
                gameObject
            );

            StopOfficer();
            return;
        }

        if (float.IsInfinity(agent.remainingDistance))
            return;

        bool reachedDestination =
            agent.remainingDistance <= agent.stoppingDistance &&
            (!agent.hasPath ||
             agent.velocity.sqrMagnitude <= 0.01f);

        if (reachedDestination)
            StopOfficer();
    }

    public bool InterceptPlayer()
    {
        if (activated || stopped)
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
                "Police officer is not standing on a NavMesh.",
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
                "No NavMesh was found near OfficerStopPoint.",
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

        if (!pathFound ||
            calculatedPath.status != NavMeshPathStatus.PathComplete)
        {
            Debug.LogError(
                "The officer does not have a complete path to " +
                "OfficerStopPoint. Path status: " +
                calculatedPath.status,
                gameObject
            );

            return false;
        }

        activated = true;
        stopped = false;
        canInteract = false;

        agent.ResetPath();
        agent.isStopped = false;
        agent.SetPath(calculatedPath);

        return true;
    }

    private void StopOfficer()
    {
        if (stopped)
            return;

        stopped = true;
        activated = false;
        canInteract = true;

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
            "Officer has arrived. Player can now interact.",
            gameObject
        );
    }

    private void FacePlayer()
    {
        if (!facePlayerAfterStopping || player == null)
            return;

        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.001f)
            return;

        Quaternion targetRotation =
            Quaternion.LookRotation(direction.normalized);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    private void UpdateAnimation()
    {
        if (officerAnimator == null)
            return;

        float movementSpeed = 0f;

        if (agent != null && agent.isOnNavMesh)
            movementSpeed = agent.velocity.magnitude;

        if (HasAnimatorParameter("Speed"))
        {
            officerAnimator.SetFloat(
                "Speed",
                movementSpeed
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