/*
 * Author: Joyce Kwek
 * Date: 10th August 2026
 * File: NPCGirl.cs
 * Description:
 * Controls NPCGirl during the unsafe road-crossing demonstration.
 * NPCGirl waits beside the pedestrian crossing and crosses the road
 * once the player reaches the WrongWayDialogue trigger.
 */

using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Controls NPCGirl and demonstrates unsafe pedestrian behaviour
/// by crossing the road while the pedestrian signal is red.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class NPCGirl : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform crossingDestination;
    [SerializeField] private TrafficLightControl trafficLight;

    [Header("Movement")]
    [SerializeField] private float runningSpeed = 4.5f;
    [SerializeField] private float stoppingDistance = 0.2f;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private string speedParameter = "Speed";

    private bool isCrossing;
    private bool hasCrossed;

    /// <summary>
    /// Returns whether NPCGirl has completed the unsafe crossing.
    /// </summary>
    public bool HasCrossed => hasCrossed;

    /// <summary>
    /// Gets the required components and prepares NPCGirl
    /// to wait beside the pedestrian crossing.
    /// </summary>
    private void Awake()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        agent.speed = runningSpeed;
        agent.stoppingDistance = stoppingDistance;
        agent.autoBraking = true;

        if (animator != null)
            animator.applyRootMotion = false;

        if (agent.isOnNavMesh)
            agent.isStopped = true;
    }

    /// <summary>
    /// Updates NPCGirl's movement and detects when
    /// she reaches the opposite side of the road.
    /// </summary>
    private void Update()
    {
        UpdateAnimation();

        if (!isCrossing)
            return;

        if (!agent.isOnNavMesh)
            return;

        if (agent.pathPending)
            return;

        if (agent.pathStatus == NavMeshPathStatus.PathPartial)
        {
            Debug.LogError(
                "NPCGirl has a partial path. Check the NavMesh across the road.",
                gameObject
            );

            return;
        }

        if (agent.pathStatus == NavMeshPathStatus.PathInvalid)
        {
            Debug.LogError(
                "NPCGirl has an invalid NavMesh path.",
                gameObject
            );

            return;
        }

        if (float.IsInfinity(agent.remainingDistance))
            return;

        bool reachedDestination =
            agent.remainingDistance <= agent.stoppingDistance &&
            (!agent.hasPath ||
             agent.velocity.sqrMagnitude <= 0.01f);

        if (reachedDestination)
            FinishCrossing();
    }

    /// <summary>
    /// Starts NPCGirl crossing the road while the
    /// pedestrian traffic light is red.
    /// </summary>
    public void StartWrongCrossing()
    {
        if (isCrossing || hasCrossed)
            return;

        Debug.Log(
            "NPCGirl StartWrongCrossing triggered.",
            gameObject
        );

        if (crossingDestination == null)
        {
            Debug.LogError(
                "WrongWayCrossPoint has not been assigned.",
                gameObject
            );

            return;
        }

        if (trafficLight == null)
        {
            Debug.LogError(
                "TrafficPole has not been assigned to NPCGirl.",
                gameObject
            );

            return;
        }

        if (!agent.isOnNavMesh)
        {
            Debug.LogError(
                "NPCGirl is not standing on the NavMesh.",
                gameObject
            );

            return;
        }

        if (trafficLight.CurrentState !=
            TrafficLightControl.LightState.Red)
        {
            Debug.LogWarning(
                "NPCGirl demonstration requires the light to be RED.",
                gameObject
            );

            return;
        }

        NavMeshPath path = new NavMeshPath();

        bool pathFound = agent.CalculatePath(
            crossingDestination.position,
            path
        );

        if (!pathFound ||
            path.status != NavMeshPathStatus.PathComplete)
        {
            Debug.LogError(
                "NPCGirl cannot find a complete path to WrongWayCrossPoint. " +
                "Path status: " + path.status,
                gameObject
            );

            return;
        }

        agent.speed = runningSpeed;
        agent.isStopped = false;
        agent.SetPath(path);

        isCrossing = true;

        Debug.Log(
            "NPCGirl is crossing while the light is RED.",
            gameObject
        );
    }

    /// <summary>
    /// Stops NPCGirl after she reaches the opposite pavement.
    /// </summary>
    private void FinishCrossing()
    {
        isCrossing = false;
        hasCrossed = true;

        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        agent.ResetPath();

        UpdateAnimation();

        Debug.Log(
            "NPCGirl completed the unsafe crossing.",
            gameObject
        );
    }

    /// <summary>
    /// Updates NPCGirl's movement animation.
    /// </summary>
    private void UpdateAnimation()
    {
        if (animator == null)
            return;

        if (!HasAnimatorParameter(speedParameter))
            return;

        animator.SetFloat(
            speedParameter,
            agent.velocity.magnitude
        );
    }

    /// <summary>
    /// Checks whether the NPC Animator contains
    /// the requested parameter.
    /// </summary>
    private bool HasAnimatorParameter(string parameterName)
    {
        if (animator == null)
            return false;

        foreach (AnimatorControllerParameter parameter
                 in animator.parameters)
        {
            if (parameter.name == parameterName)
                return true;
        }

        return false;
    }
}