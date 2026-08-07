using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    

    [Header("Player Settings")]
    public float interactionDistance = 2.5f;
    public Camera playerCamera;
    
    [Header("Phone UI / Model")]
    public GameObject phoneObject; // Player's phone visual object

    private bool isPhoneEquipped = true;

    void Update()
    {
        HandleSequenceInputs();
    }

    private void HandleSequenceInputs()
    {
        switch (currentStep)
        {
            case CrossingStep.NotStarted:
                // Stop at the sidewalk edge
                if (Vector3.Distance(transform.position, sidewalkEdgePoint.position) <= interactionDistance)
                {
                    AdvanceStep(CrossingStep.StoppedAtSidewalk, "Stopped at the sidewalk edge.");
                }
                break;

            case CrossingStep.StoppedAtSidewalk:
                //Put away the smartphone
                if (Input.GetKeyDown(KeyCode.P) && isPhoneEquipped) // Press 'P' to stow phone
                {
                    isPhoneEquipped = false;
                    if (phoneObject != null) phoneObject.SetActive(false);
                    AdvanceStep(CrossingStep.PhonePutAway, "Put away smartphone.");
                }
                break;

            case CrossingStep.PhonePutAway:
                // Wait at the pedestrian crossing
                if (Vector3.Distance(transform.position, pedestrianCrossingPoint.position) <= interactionDistance)
                {
                    AdvanceStep(CrossingStep.WaitingAtCrossing, "Reached pedestrian crossing point.");
                }
                break;

            case CrossingStep.WaitingAtCrossing:
                // Observe the pedestrian traffic signal
                if (Input.GetKeyDown(KeyCode.E)) // Press 'E' to inspect light
                {
                    RaycastHit hit;
                    if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, interactionDistance))
                    {
                        if (hit.collider.CompareTag("TrafficLight"))
                        {
                            if (trafficLight != null && trafficLight.IsGreenForPedestrians())
                            {
                                AdvanceStep(CrossingStep.ObservedTrafficSignal, "Signal observed: Walk signal is green.");
                            }
                            else
                            {
                                Debug.Log("Signal is Red! Wait before looking around.");
                            }
                        }
                    }
                }
                break;

            case CrossingStep.ObservedTrafficSignal:
                // Look Right
                if (IsLookingInDirection(Vector3.right))
                {
                    AdvanceStep(CrossingStep.LookedRight1, "Looked Right.");
                }
                break;

            case CrossingStep.LookedRight1:
                // Look Left
                if (IsLookingInDirection(Vector3.left))
                {
                    AdvanceStep(CrossingStep.LookedLeft, "Looked Left.");
                }
                break;

            case CrossingStep.LookedLeft:
                // Look Right Again
                if (IsLookingInDirection(Vector3.right))
                {
                    AdvanceStep(CrossingStep.LookedRight2, "Looked Right Again.");
                }
                break;

            case CrossingStep.LookedRight2:
                // Listen for approaching vehicles (Hold key or complete observation)
                if (Input.GetKeyDown(KeyCode.L)) // Press 'L' to listen
                {
                    AdvanceStep(CrossingStep.ListenedForVehicles, "Listened for oncoming traffic/engine sounds.");
                }
                break;

            case CrossingStep.ListenedForVehicles:
                // Cross only when safe
                AdvanceStep(CrossingStep.CrossingSafe, "Proceed safely across the street!");
                break;

            case CrossingStep.CrossingSafe:
                // Successfully reach destination (Bus Stop)
                if (Vector3.Distance(transform.position, busStopPoint.position) <= interactionDistance)
                {
                    AdvanceStep(CrossingStep.ReachedBusStop, "Successfully reached the bus stop! Sequence Complete.");
                }
                break;

            case CrossingStep.ReachedBusStop:
                // Task Complete
                break;
        }
    }

    private bool IsLookingInDirection(Vector3 targetDirection)
    {
        // Compares player camera forward direction against world direction vectors
        float dotProduct = Vector3.Dot(playerCamera.transform.forward, transform.TransformDirection(targetDirection));
        return dotProduct > 0.75f; // Player is looking roughly toward that relative direction
    }

    private void AdvanceStep(CrossingStep nextStep, string message)
    {
        currentStep = nextStep;
        Debug.Log($"[Road Safety Task]: {message}");
    }
}