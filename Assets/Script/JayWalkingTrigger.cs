using UnityEngine;

public class JaywalkingTrigger : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PoliceOfficer policeOfficer;

    private bool hasTriggered;

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