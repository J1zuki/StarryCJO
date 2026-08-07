using UnityEngine;
using System.Collections;

/// <summary>
/// Controls pedestrian light cycles, button interactions, and visual indicators.
/// </summary>
public class TrafficPole : MonoBehaviour
{
    public enum PedestrianSignalState
    {
        DontWalk, // Red light for pedestrians
        Walk      // Green light for pedestrians
    }

    [Header("Current State")]
    public PedestrianSignalState currentPedestrianState = PedestrianSignalState.DontWalk;

    [Header("Timing Settings")]
    public float redLightDuration = 5.0f;
    public float greenLightDuration = 8.0f;

    [Header("Visual Indicators")]
    public GameObject redLightVisual;
    public GameObject greenLightVisual;

    private Coroutine lightCycleCoroutine;

    private void Awake()
    {
        // Ensures raycasts tag detection works seamlessly
        if (!gameObject.CompareTag("TrafficLight"))
        {
            gameObject.tag = "TrafficLight";
        }
    }

    private void Start()
    {
        lightCycleCoroutine = StartCoroutine(TrafficLightCycle());
    }

    private IEnumerator TrafficLightCycle()
    {
        while (true)
        {
            // Red Phase (Don't Walk)
            SetPedestrianState(PedestrianSignalState.DontWalk);
            yield return new WaitForSeconds(redLightDuration);

            // Green Phase (Walk)
            SetPedestrianState(PedestrianSignalState.Walk);
            yield return new WaitForSeconds(greenLightDuration);
        }
    }

    private void SetPedestrianState(PedestrianSignalState newState)
    {
        currentPedestrianState = newState;

        if (redLightVisual != null) 
            redLightVisual.SetActive(newState == PedestrianSignalState.DontWalk);

        if (greenLightVisual != null) 
            greenLightVisual.SetActive(newState == PedestrianSignalState.Walk);
    }

    /// <summary>
    /// Forces the walk light immediately when interacting with the button pole.
    /// </summary>
    public void RequestWalkSignal()
    {
        if (currentPedestrianState == PedestrianSignalState.DontWalk)
        {
            if (lightCycleCoroutine != null)
                StopCoroutine(lightCycleCoroutine);

            SetPedestrianState(PedestrianSignalState.Walk);
            lightCycleCoroutine = StartCoroutine(ResetAfterWalk());
        }
    }

    private IEnumerator ResetAfterWalk()
    {
        yield return new WaitForSeconds(greenLightDuration);
        lightCycleCoroutine = StartCoroutine(TrafficLightCycle());
    }

    public bool IsGreenForPedestrians()
    {
        return currentPedestrianState == PedestrianSignalState.Walk;
    }
}