/*
 * Author: Joyce Kwek
 * Date: 10th August 2026
 * File: BusStopCrossPoint.cs
 * Description:
 * Detects when the player reaches the bus-stop destination
 * after demonstrating the correct road-crossing method.
 */

using UnityEngine;

/// <summary>
/// Completes the correct-crossing mission when
/// PlayerCapsule reaches the bus-stop destination.
/// </summary>
public class BusStopCrossPoint : MonoBehaviour
{
    [Header("Mission")]
    [SerializeField]
    private CorrectCrossingMission missionManager;

    private bool hasTriggered;

    /// <summary>
    /// Detects PlayerCapsule entering BusStopCrossPoint
    /// and completes the correct crossing mission.
    /// </summary>
    /// <param name="other">
    /// Collider that entered the trigger.
    /// </param>
    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered)
            return;

        if (!other.CompareTag("Player"))
            return;

        if (missionManager == null)
        {
            Debug.LogError(
                "CorrectCrossingMission has not been assigned.",
                gameObject
            );

            return;
        }

        if (!missionManager.PlayerCanDemonstrate)
            return;

        hasTriggered = true;

        missionManager.CompleteCorrectCrossing();

        Debug.Log(
            "Player reached BusStopCrossPoint.",
            gameObject
        );
    }
}