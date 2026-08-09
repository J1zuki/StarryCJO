using System.Collections;
using TMPro;
using UnityEngine;

public class NPCAccidentSequence : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TrafficLightControl trafficLight;
    [SerializeField] private GameObject jaywalkingNPC;
    [SerializeField] private Transform npcTargetPoint;
    [SerializeField] private GameObject speedingCar;
    [SerializeField] private Transform carCrashPoint;

    [Header("Fire & Smoke FX")]
    [SerializeField] private GameObject fireAndSmokeFX; // Drag your Particle System prefab/object here

    [Header("Dialogue & UI")]
    [SerializeField] private TMP_Text floatingMiniDialogue; 
    [SerializeField] private PoliceOfficerInteraction policeDialogueScript;

    [Header("Settings")]
    [SerializeField] private float npcWalkSpeed = 2f;
    [SerializeField] private float carSpeed = 25f;

    private bool sequenceTriggered = false;

    private void Start()
    {
        if (fireAndSmokeFX != null) fireAndSmokeFX.SetActive(false);
        if (speedingCar != null) speedingCar.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (sequenceTriggered) return;

        if (other.CompareTag("Player"))
        {
            // Only trigger if light is still RED
            if (trafficLight != null && trafficLight.currentState == TrafficLightControl.LightState.Red)
            {
                sequenceTriggered = true;
                StartCoroutine(RunAccidentSequence());
            }
        }
    }

    private IEnumerator RunAccidentSequence()
    {
        // 1. NPC begins walking across on RED light without looking
        if (floatingMiniDialogue != null)
        {
            floatingMiniDialogue.gameObject.SetActive(true);
            floatingMiniDialogue.text = "This is the wrong way of crossing a pedestrian, especially during red light. Now, let me show you the right way.";
        }

        Vector3 npcStartPos = jaywalkingNPC.transform.position;
        float walkProgress = 0f;
        float totalDistance = Vector3.Distance(npcStartPos, npcTargetPoint.position);

        // Make NPC look straight ahead towards target (never left or right)
        jaywalkingNPC.transform.LookAt(npcTargetPoint);

        bool impactOccurred = false;

        while (walkProgress < 1f && !impactOccurred)
        {
            walkProgress += Time.deltaTime * (npcWalkSpeed / totalDistance);
            jaywalkingNPC.transform.position = Vector3.Lerp(npcStartPos, npcTargetPoint.position, walkProgress);

            // Spawn car when NPC reaches ~30% of the crosswalk
            if (walkProgress >= 0.3f && speedingCar != null && !speedingCar.activeSelf)
            {
                speedingCar.SetActive(true);
            }

            // Move speeding car toward impact point
            if (speedingCar != null && speedingCar.activeSelf)
            {
                speedingCar.transform.position = Vector3.MoveTowards(
                    speedingCar.transform.position,
                    carCrashPoint.position,
                    carSpeed * Time.deltaTime
                );

                // Check distance for impact
                if (Vector3.Distance(speedingCar.transform.position, carCrashPoint.position) < 0.5f)
                {
                    impactOccurred = true;
                }
            }

            yield return null;
        }

        // 2. CRASH / IMPACT OCCURS
        if (fireAndSmokeFX != null)
        {
            fireAndSmokeFX.transform.position = carCrashPoint.position;
            fireAndSmokeFX.SetActive(true); // Fire and smoke turn on
        }

        // Hide or ragdoll the NPC on impact
        if (jaywalkingNPC != null)
        {
            jaywalkingNPC.SetActive(false);
        }

        yield return new WaitForSeconds(2.5f);

        // 3. Clear floating text & trigger main dialogue script
        if (floatingMiniDialogue != null)
        {
            floatingMiniDialogue.gameObject.SetActive(false);
        }

        if (policeDialogueScript != null)
        {
            policeDialogueScript.ChooseWhy(); 
        }
    }
}