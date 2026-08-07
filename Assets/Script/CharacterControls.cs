using System.Collections;
using UnityEngine;
using TMPro;

/// <summary>
/// Manages the step-by-step safe road crossing sequence.
/// Order: Stop -> Put Away Phone -> Wait at Crossing -> Check Light -> Look R -> Look L -> Look R -> Listen -> Cross -> Bus Stop
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

    [Header("Detection Points")]
    public Transform sidewalkEdgePoint;
    public Transform pedestrianCrossingPoint;
    public Transform busStopPoint;
    public TrafficPole trafficLight;

    [Header("Player Settings")]
    public float interactionDistance = 3.0f;
    public Camera playerCamera;
    
    [Header("Phone Setup")]
    public GameObject phoneObject;
    private bool isPhoneEquipped = true;

    [Header("Optional UI Instruction Text")]
    public TMP_Text instructionText;

    private void Start()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

        UpdateUIInstruction("Step 1: Walk to the sidewalk edge to stop.");
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
                if (sidewalkEdgePoint != null && Vector3.Distance(transform.position, sidewalkEdgePoint.position) <= interactionDistance)
                {
                    AdvanceStep(CrossingStep.StoppedAtSidewalk, "Stopped at the sidewalk edge.", "Press [P] to put away your phone.");
                }
                break;

            case CrossingStep.StoppedAtSidewalk:
                if (Input.GetKeyDown(KeyCode.P) && isPhoneEquipped)
                {
                    isPhoneEquipped = false;
                    if (phoneObject != null) phoneObject.SetActive(false);
                    AdvanceStep(CrossingStep.PhonePutAway, "Put away smartphone.", "Walk to the pedestrian crossing point.");
                }
                break;

            case CrossingStep.PhonePutAway:
                if (pedestrianCrossingPoint != null && Vector3.Distance(transform.position, pedestrianCrossingPoint.position) <= interactionDistance)
                {
                    AdvanceStep(CrossingStep.WaitingAtCrossing, "Reached pedestrian crossing point.", "Look at the Traffic Signal and press [E].");
                }
                break;

            case CrossingStep.WaitingAtCrossing:
                if (Input.GetKeyDown(KeyCode.P))
                {
                    Debug.Log("[Road Safety]: You pressed [P] to put away your phone.");
                    isPhoneEquipped = false;
                    if (phoneObject != null) phoneObject.SetActive(false);
                }
                {
                    if (playerCamera == null) playerCamera = Camera.main;

                    RaycastHit hit;
                    Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

                    if (Physics.Raycast(ray, out hit, interactionDistance * 2f))
                    {
                        TrafficPole pole = hit.collider.GetComponentInParent<TrafficPole>();
                        if (pole != null || hit.collider.CompareTag("TrafficLight"))
                        {
                            if (trafficLight != null && trafficLight.IsGreenForPedestrians())
                            {
                                AdvanceStep(CrossingStep.ObservedTrafficSignal, "Signal observed: Walk signal is GREEN.", "Turn your head to look RIGHT.");
                            }
                            else
                            {
                                Debug.Log("[Road Safety]: Signal is RED/DONT WALK! Wait until green.");
                                UpdateUIInstruction("Signal is RED! Wait for green walk signal then press [E].");
                            }
                        }
                    }
                    else
                    {
                        // Fallback check based on proximity if raycast misses
                        if (trafficLight != null && trafficLight.IsGreenForPedestrians())
                        {
                            AdvanceStep(CrossingStep.ObservedTrafficSignal, "Signal observed: Walk signal is GREEN.", "Turn your head to look RIGHT.");
                        }
                        else
                        {
                            UpdateUIInstruction("Look closely at the traffic pole and press [E].");
                        }
                    }
                }
                break;

            case CrossingStep.ObservedTrafficSignal:
                if (IsLookingInDirection(Vector3.right))
                {
                    AdvanceStep(CrossingStep.LookedRight1, "Looked Right.", "Now turn your camera to look LEFT.");
                }
                break;

            case CrossingStep.LookedRight1:
                if (IsLookingInDirection(Vector3.left))
                {
                    AdvanceStep(CrossingStep.LookedLeft, "Looked Left.", "Now look RIGHT again.");
                }
                break;

            case CrossingStep.LookedLeft:
                if (IsLookingInDirection(Vector3.right))
                {
                    AdvanceStep(CrossingStep.LookedRight2, "Looked Right Again.", "Press [L] to listen for approaching vehicles.");
                }
                break;

            case CrossingStep.LookedRight2:
                if (Input.GetKeyDown(KeyCode.L))
                {
                    AdvanceStep(CrossingStep.ListenedForVehicles, "Listened for oncoming traffic.", "Road is clear! Safely cross the street.");
                }
                break;

            case CrossingStep.ListenedForVehicles:
                AdvanceStep(CrossingStep.CrossingSafe, "Proceed safely across the street!", "Head to the Bus Stop.");
                break;

            case CrossingStep.CrossingSafe:
                if (busStopPoint != null && Vector3.Distance(transform.position, busStopPoint.position) <= interactionDistance)
                {
                    AdvanceStep(CrossingStep.ReachedBusStop, "Reached the bus stop!", "Sequence Complete! You crossed safely.");
                }
                break;

            case CrossingStep.ReachedBusStop:
                break;
        }
    }

    private bool IsLookingInDirection(Vector3 targetDirection)
    {
        if (playerCamera == null) return false;

        Vector3 cameraForward = playerCamera.transform.forward;
        cameraForward.y = 0; // Look strictly on the horizontal plane
        cameraForward.Normalize();

        Vector3 localTargetDir = transform.TransformDirection(targetDirection);
        localTargetDir.y = 0;
        localTargetDir.Normalize();

        float dotProduct = Vector3.Dot(cameraForward, localTargetDir);
        return dotProduct > 0.65f;
    }

    private void AdvanceStep(CrossingStep nextStep, string logMessage, string nextInstruction = "")
    {
        currentStep = nextStep;
        Debug.Log($"[Road Safety Task]: {logMessage}");
        UpdateUIInstruction(nextInstruction);
    }

    private void UpdateUIInstruction(string text)
    {
        if (instructionText != null && !string.IsNullOrEmpty(text))
        {
            instructionText.text = text;
        }
    }
}