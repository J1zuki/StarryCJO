/*
 * Author: Joyce Kwek
 * Date: 10th August 2026
 * File: BusStopCrossPoint.cs
 * Description:
 * Detects when PlayerCapsule reaches the bus stop
 * after completing the correct road-crossing demonstration.
 * It then tells CorrectCrossingMission to show
 * the completion UI and award points.
 */

using UnityEngine;

/// <summary>
/// Completes the correct crossing mission when
/// PlayerCapsule enters BusStopCrossPoint.
/// </summary>
public class BusStopCrossPoint : MonoBehaviour
{
    [Header("Mission")]
    [SerializeField]
    private CorrectCrossingMission missionManager;

    private bool hasTriggered;

    /// <summary>
    /// Detects when PlayerCapsule enters
    /// the BusStopCrossPoint trigger.
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
                "CorrectCrossingMission has not been assigned to BusStopCrossPoint.",
                gameObject
            );

            return;
        }

        if (!missionManager.PlayerCanDemonstrate)
        {
            Debug.LogWarning(
                "Player reached BusStopCrossPoint before the correct crossing demonstration started.",
                gameObject
            );

            return;
        }

        hasTriggered = true;

        missionManager.CompleteCorrectCrossing();

        Debug.Log(
            "Player reached BusStopCrossPoint. Completion UI opened.",
            gameObject
        );
    }
}