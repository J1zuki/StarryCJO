/*
 * Author: Joyce Kwek
 * Date: 10th August 2026
 * File: NPCGirl.cs
 * Description:
 * Controls NPCGirl during the unsafe crossing demonstration.
 * NPCGirl crosses the road on red and can later reset
 * to her original starting position.
 */

using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Controls NPCGirl's unsafe crossing demonstration
/// and allows her to return to her original position.
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

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    /// <summary>
    /// Returns whether NPCGirl has completed
    /// the unsafe crossing demonstration.
    /// </summary>
    public bool HasCrossed => hasCrossed;

    /// <summary>
    /// Saves NPCGirl's original position and prepares
    /// the NavMeshAgent when the scene begins.
    /// </summary>
    private void Awake()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        originalPosition = transform.position;
        originalRotation = transform.rotation;

        agent.speed = runningSpeed;
        agent.stoppingDistance = stoppingDistance;
        agent.autoBraking = true;

        if (animator != null)
            animator.applyRootMotion = false;

        if (agent.isOnNavMesh)
        {
            agent.isStopped = true;
        }
        else
        {
            Debug.LogError(
                "NPCGirl is not standing on the NavMesh.",
                gameObject
            );
        }
    }

    /// <summary>
    /// Updates NPCGirl's animation and checks whether
    /// she has reached WrongWayCrossPoint.
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

        if (agent.pathStatus == NavMeshPathStatus.PathInvalid)
        {
            Debug.LogError(
                "NPCGirl path is invalid.",
                gameObject
            );

            StopNPC();
            return;
        }

        if (agent.pathStatus == NavMeshPathStatus.PathPartial)
        {
            Debug.LogError(
                "NPCGirl path is partial. Check the NavMesh connection.",
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
            FinishWrongCrossing();
    }

    /// <summary>
    /// Starts NPCGirl crossing the road while
    /// the pedestrian traffic signal is red.
    /// </summary>
    public void StartWrongCrossing()
    {
        if (isCrossing || hasCrossed)
            return;

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
                "Traffic Light has not been assigned.",
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
                "NPCGirl should only cross while the pedestrian light is RED.",
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
            "NPCGirl started the unsafe crossing.",
            gameObject
        );
    }

    /// <summary>
    /// Stops NPCGirl after she reaches
    /// WrongWayCrossPoint.
    /// </summary>
    private void FinishWrongCrossing()
    {
        isCrossing = false;
        hasCrossed = true;

        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        agent.ResetPath();

        UpdateAnimation();

        Debug.Log(
            "NPCGirl reached WrongWayCrossPoint.",
            gameObject
        );
    }

    /// <summary>
    /// Returns NPCGirl to her exact original
    /// position and rotation.
    /// </summary>
    public void ResetToOriginalPosition()
    {
        if (!agent.isOnNavMesh)
        {
            Debug.LogError(
                "NPCGirl cannot reset because she is not on the NavMesh.",
                gameObject
            );

            return;
        }

        agent.isStopped = true;
        agent.ResetPath();

        bool warped =
            agent.Warp(originalPosition);

        if (!warped)
        {
            Debug.LogError(
                "NPCGirl could not return to her original position.",
                gameObject
            );

            return;
        }

        transform.rotation = originalRotation;

        agent.velocity = Vector3.zero;

        isCrossing = false;
        hasCrossed = false;

        UpdateAnimation();

        Debug.Log(
            "NPCGirl returned to her original position.",
            gameObject
        );
    }

    /// <summary>
    /// Stops NPCGirl and clears the current NavMesh path.
    /// </summary>
    private void StopNPC()
    {
        isCrossing = false;

        if (!agent.isOnNavMesh)
            return;

        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        agent.ResetPath();

        UpdateAnimation();
    }

    /// <summary>
    /// Updates NPCGirl's Animator Speed parameter
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
    /// Checks whether NPCGirl's Animator contains
    /// the specified parameter.
    /// </summary>
    /// <param name="parameterName">
    /// Animator parameter to check.
    /// </param>
    /// <returns>
    /// True when the parameter exists.
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