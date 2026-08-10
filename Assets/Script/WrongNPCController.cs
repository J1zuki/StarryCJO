/*
 * Author: Joyce Kwek
 * Date: 10th August 2026
 * File: WrongNPCController.cs
 * Description:
 * Controls the unsafe pedestrian NPC demonstration.
 * The NPC waits at the traffic light and runs across the road
 * when instructed by the WrongWayDialogue trigger.
 */

using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Controls the NPC that demonstrates unsafe behaviour
/// by crossing the road while the pedestrian traffic light is red.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class WrongNPCController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform crossingDestination;
    [SerializeField] private TrafficLightControl trafficLight;

    [Header("Movement")]
    [SerializeField] private float runningSpeed = 5f;
    [SerializeField] private float stoppingDistance = 0.2f;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private string speedParameter = "Speed";

    private bool isCrossing;
    private bool hasFinished;

    /// <summary>
    /// Gets the required components and prepares the NPC
    /// to wait at the pedestrian crossing.
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

        if (agent.isOnNavMesh)
            agent.isStopped = true;
    }

    /// <summary>
    /// Checks the NPC movement and stops the NPC
    /// after reaching the crossing destination.
    /// </summary>
    private void Update()
    {
        UpdateAnimation();

        if (!isCrossing || hasFinished)
            return;

        if (!agent.isOnNavMesh)
            return;

        if (agent.pathPending)
            return;

        if (agent.pathStatus != NavMeshPathStatus.PathComplete)
        {
            Debug.LogError(
                "Wrong NPC does not have a complete NavMesh path.",
                gameObject
            );

            StopNPC();
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
    /// Starts the unsafe crossing demonstration only if
    /// the pedestrian traffic light is currently red.
    /// </summary>
    public void StartWrongCrossing()
    {
        if (isCrossing || hasFinished)
            return;

        if (crossingDestination == null)
        {
            Debug.LogError(
                "Wrong NPC Crossing Destination has not been assigned.",
                gameObject
            );

            return;
        }

        if (trafficLight == null)
        {
            Debug.LogError(
                "Traffic Light has not been assigned to WrongNPCController.",
                gameObject
            );

            return;
        }

        if (!agent.isOnNavMesh)
        {
            Debug.LogError(
                "Wrong NPC is not standing on the NavMesh.",
                gameObject
            );

            return;
        }

        if (trafficLight.CurrentState !=
            TrafficLightControl.LightState.Red)
        {
            Debug.LogWarning(
                "Wrong NPC can only start crossing while the pedestrian light is red.",
                gameObject
            );

            return;
        }

        NavMeshPath path = new NavMeshPath();

        bool pathFound =
            agent.CalculatePath(
                crossingDestination.position,
                path
            );

        if (!pathFound ||
            path.status != NavMeshPathStatus.PathComplete)
        {
            Debug.LogError(
                "Wrong NPC cannot find a complete path to the crossing destination.",
                gameObject
            );

            return;
        }

        agent.speed = runningSpeed;
        agent.stoppingDistance = stoppingDistance;
        agent.isStopped = false;

        agent.SetPath(path);

        isCrossing = true;

        Debug.Log(
            "Wrong NPC started crossing while the pedestrian light is RED.",
            gameObject
        );
    }

    /// <summary>
    /// Stops the NPC after it reaches the other side
    /// of the road.
    /// </summary>
    private void FinishCrossing()
    {
        hasFinished = true;
        isCrossing = false;

        StopNPC();

        Debug.Log(
            "Wrong NPC finished the unsafe crossing.",
            gameObject
        );
    }

    /// <summary>
    /// Stops the NavMeshAgent and clears its current path.
    /// </summary>
    private void StopNPC()
    {
        if (!agent.isOnNavMesh)
            return;

        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        agent.ResetPath();

        UpdateAnimation();
    }

    /// <summary>
    /// Updates the NPC Animator Speed parameter
    /// using the NavMeshAgent movement speed.
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
    /// a parameter with the specified name.
    /// </summary>
    /// <param name="parameterName">
    /// The name of the Animator parameter.
    /// </param>
    /// <returns>
    /// Returns true when the parameter exists.
    /// </returns>
    private bool HasAnimatorParameter(
        string parameterName)
    {
        if (animator == null)
            return false;

        foreach (
            AnimatorControllerParameter parameter
            in animator.parameters)
        {
            if (parameter.name == parameterName)
                return true;
        }

        return false;
    }
}