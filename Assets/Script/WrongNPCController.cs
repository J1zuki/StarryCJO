/*
 * Author: Joyce Kwek Siok Teng
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
/// Controls the NPC that demonstrates the unsafe behaviour
/// of crossing the road while the pedestrian traffic light is red.
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
    /// Checks the NPC's movement and stops the NPC
    /// after reaching the opposite side of the road.
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
    /// Starts the unsafe crossing demonstration only when
    /// the pedestrian traffic signal is currently red.
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

        if (!agent.isOnNavMesh)
        {
            Debug.LogError(
                "Wrong NPC is not standing on the NavMesh.",
                gameObject
            );

            return;
        }

        if (trafficLight != null &&
            trafficLight.CurrentState !=
            TrafficLightControl.LightState.Red)
        {
            Debug.LogWarning(
                "Wrong NPC demonstration can only start while the light is red.",
                gameObject
            );

            return;
        }

        agent.speed = runningSpeed;
        agent.isStopped = false;

        bool pathStarted =
            agent.SetDestination(
                crossingDestination.position
            );

        if (!pathStarted)
        {
            Debug.LogError(
                "Wrong NPC could not create a path to the crossing destination.",
                gameObject
            );

            return;
        }

        isCrossing = true;

        Debug.Log(
            "Wrong NPC is crossing while the pedestrian signal is RED.",
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

        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        agent.ResetPath();

        UpdateAnimation();

        Debug.Log(
            "Wrong NPC finished the unsafe crossing.",
            gameObject
        );
    }

    /// <summary>
    /// Updates the NPC's Animator Speed parameter
    /// using the current NavMeshAgent movement speed.
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
    /// the specified parameter.
    /// </summary>
    /// <param name="parameterName">
    /// Name of the Animator parameter.
    /// </param>
    /// <returns>
    /// True if the Animator parameter exists.
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