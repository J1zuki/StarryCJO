using UnityEngine;
using System.Collections;

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

    [Header("Visual Indicators (Optional)")]
    public GameObject redLightVisual;
    public GameObject greenLightVisual;

    private void Start()
    {
        StartCoroutine(TrafficLightCycle());
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

        // Toggle visual indicators if assigned
        if (redLightVisual != null) 
            redLightVisual.SetActive(newState == PedestrianSignalState.DontWalk);

        if (greenLightVisual != null) 
            greenLightVisual.SetActive(newState == PedestrianSignalState.Walk);
    }

    /// <summary>
    /// Called by RoadCrossingManager to check if it's safe to cross.
    /// </summary>
    public bool IsGreenForPedestrians()
    {
        return currentPedestrianState == PedestrianSignalState.Walk;
    }
}