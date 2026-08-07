using System.Collections;
using UnityEngine;

/// <summary>
/// Controls the player's progression through the step-by-step 
/// safe road-crossing procedure leading up to the bus stop.
/// </summary>
public class CharacterControls : MonoBehaviour
{
    public enum CrossingStep
    {
        NotStarted,
        StoppedAtSidewalk,
        PhonePutAway,
        WaitingAtCrossing,
        ObservedTrafficSignal,
        LookedRight1,
        LookedLeft,
        LookedRight2,
        ListenedForVehicles,
        CrossingSafe,
        ReachedBusStop
    }

    [Header("Current Sequence State")]
    public CrossingStep currentStep = CrossingStep.NotStarted;

    [Header("Detection Triggers & Targets")]
    public Transform sidewalkEdgePoint;
    public Transform pedestrianCrossingPoint;
    public Transform busStopPoint;
    public TrafficPole trafficLight;

    [Header("Player Settings")]
    public float interactionDistance = 3.0f;
    public Camera playerCamera;

    [Header("Phone UI / Model")]
    public GameObject phoneObject; // Player's phone visual object

    private bool isPhoneEquipped = true;

    private void Start()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }
    }

    private void Update()
    {
        HandleSequenceInputs();
    }

    private void HandleSequenceInputs()
    {
        switch (currentStep)
        {
            case CrossingStep.NotStarted:
                // Step 1: Stop at the edge of the sidewalk
                if (sidewalkEdgePoint != null && Vector3.Distance(transform.position, sidewalkEdgePoint.position) <= interactionDistance)
                {
                    AdvanceStep(CrossingStep.StoppedAtSidewalk, "Step 1 Complete: Stopped at the edge of the sidewalk. Press 'P' to put away your phone.");
                }
                break;

            case CrossingStep.StoppedAtSidewalk:
                // Step 2: Put away the smartphone
                if (Input.GetKeyDown(KeyCode.P) && isPhoneEquipped)
                {
                    isPhoneEquipped = false;
                    if (phoneObject != null)
                    {
                        phoneObject.SetActive(false);
                    }
                    AdvanceStep(CrossingStep.PhonePutAway, "Step 2 Complete: Put away smartphone. Move forward to wait at the pedestrian crossing.");
                }
                break;

            case CrossingStep.PhonePutAway:
                // Step 3: Wait at the pedestrian crossing
                if (pedestrianCrossingPoint != null && Vector3.Distance(transform.position, pedestrianCrossingPoint.position) <= interactionDistance)
                {
                    AdvanceStep(CrossingStep.WaitingAtCrossing, "Step 3 Complete: Waiting at the pedestrian crossing. Press 'E' while looking at the signal light.");
                }
                break;

            case CrossingStep.WaitingAtCrossing:
                // Step 4: Observe the pedestrian traffic signal
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (IsLookingAtTrafficLight())
                    {
                        if (trafficLight != null && trafficLight.IsGreenForPedestrians())
                        {
                            AdvanceStep(CrossingStep.ObservedTrafficSignal, "Step 4 Complete: Signal is GREEN for pedestrians. Now look RIGHT.");
                        }
                        else
                        {
                            Debug.LogWarning("[Road Safety]: Pedestrian signal is RED. Wait for the WALK signal before proceeding!");
                        }
                    }
                }
                break;

            case CrossingStep.ObservedTrafficSignal:
                // Step 5a: Look Right
                if (IsLookingInRelativeDirection(45f, 135f)) // Looking to the Right relative to character
                {
                    AdvanceStep(CrossingStep.LookedRight1, "Step 5a Complete: Looked Right. Now look LEFT.");
                }
                break;

            case CrossingStep.LookedRight1:
                // Step 5b: Look Left
                if (IsLookingInRelativeDirection(-135f, -45f)) // Looking to the Left relative to character
                {
                    AdvanceStep(CrossingStep.LookedLeft, "Step 5b Complete: Looked Left. Now look RIGHT again.");
                }
                break;

            case CrossingStep.LookedLeft:
                // Step 5c: Look Right Again
                if (IsLookingInRelativeDirection(45f, 135f)) // Looking to the Right again
                {
                    AdvanceStep(CrossingStep.LookedRight2, "Step 5c Complete: Looked Right again. Press 'L' to listen for approaching traffic.");
                }
                break;

            case CrossingStep.LookedRight2:
                // Step 6: Listen for approaching vehicles
                if (Input.GetKeyDown(KeyCode.L))
                {
                    AdvanceStep(CrossingStep.ListenedForVehicles, "Step 6 Complete: Listened for vehicles. Road is safe to cross!");
                }
                break;

            case CrossingStep.ListenedForVehicles:
                // Step 7: Cross when safe
                AdvanceStep(CrossingStep.CrossingSafe, "Step 7 Complete: Proceed safely across the street toward the Bus Stop.");
                break;

            case CrossingStep.CrossingSafe:
                // Final Goal: Reach the bus stop
                if (busStopPoint != null && Vector3.Distance(transform.position, busStopPoint.position) <= interactionDistance)
                {
                    AdvanceStep(CrossingStep.ReachedBusStop, "Sequence Complete: Successfully reached the bus stop safely!");
                }
                break;

            case CrossingStep.ReachedBusStop:
                // All steps completed successfully
                break;
        }
    }

    /// <summary>
    /// Raycasts from the player's camera to verify if they are inspecting the traffic signal.
    /// </summary>
    private bool IsLookingAtTrafficLight()
    {
        if (playerCamera == null) return false;

        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, interactionDistance * 2f))
        {
            if (hit.collider.CompareTag("TrafficLight") || hit.collider.GetComponentInParent<TrafficPole>() != null)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Checks if the camera is yawed/turned within a specific local angle range relative to the player body.
    /// </summary>
    private bool IsLookingInRelativeDirection(float minAngle, float maxAngle)
    {
        if (playerCamera == null) return false;

        // Calculate horizontal angle between camera direction and character forward direction
        Vector3 cameraForwardHorizontal = Vector3.ProjectOnPlane(playerCamera.transform.forward, Vector3.up).normalized;
        Vector3 characterForwardHorizontal = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;

        float angle = Vector3.SignedAngle(characterForwardHorizontal, cameraForwardHorizontal, Vector3.up);

        return angle >= minAngle && angle <= maxAngle;
    }

    private void AdvanceStep(CrossingStep nextStep, string message)
    {
        currentStep = nextStep;
        Debug.Log($"<color=green>[Road Safety Task]:</color> {message}");
    }
}