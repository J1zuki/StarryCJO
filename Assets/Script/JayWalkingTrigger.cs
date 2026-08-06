using UnityEngine;

/// <summary>
/// Detects when the player enters the jaywalking area and commands
/// the police officer to move towards the OfficerStopPoint.
/// </summary>
public class JaywalkingTrigger : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PoliceOfficer policeOfficer;

    private bool hasTriggered;

    /// <summary>
    /// Detects the player entering the trigger, starts the police
    /// officer's interception, and disables the trigger after success.
    /// </summary>
    /// <param name="other">
    /// The Collider that entered the jaywalking trigger.
    /// </param>
    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered)
            return;

        if (!other.CompareTag("Player"))
            return;

        if (policeOfficer == null)
        {
            Debug.LogError(
                "Police Officer has not been assigned to JaywalkingTrigger.",
                gameObject
            );

            return;
        }

        // The trigger is only marked as used if the officer
        // successfully receives a valid NavMesh destination.
        bool officerStartedMoving = policeOfficer.InterceptPlayer();

        if (!officerStartedMoving)
            return;

        hasTriggered = true;

        Debug.Log("Player entered the jaywalking detection area.");

        Collider triggerCollider = GetComponent<Collider>();

        if (triggerCollider != null)
            triggerCollider.enabled = false;
    }
}