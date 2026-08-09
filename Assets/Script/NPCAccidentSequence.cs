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

    [Header("Car Spawning")]
    [SerializeField] private Transform carStartPoint;

    private void Start()
    {
        if (fireAndSmokeFX != null) fireAndSmokeFX.SetActive(false);
        if (speedingCar != null)
        {
            if (carStartPoint != null) speedingCar.transform.position = carStartPoint.position;
            speedingCar.SetActive(false);
        }
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
        if (jaywalkingNPC == null || npcTargetPoint == null) yield break;

        if (floatingMiniDialogue != null)
        {
            floatingMiniDialogue.gameObject.SetActive(true);
            floatingMiniDialogue.text = "This is the wrong way of crossing...";
        }

        Vector3 npcStartPos = jaywalkingNPC.transform.position;
        float totalDistance = Vector3.Distance(npcStartPos, npcTargetPoint.position);
        if (totalDistance <= 0.01f) totalDistance = 1f; // Prevent divide by zero

        float walkProgress = 0f;
        jaywalkingNPC.transform.LookAt(npcTargetPoint);
        bool impactOccurred = false;

        while (walkProgress < 1f && !impactOccurred)
        {
            walkProgress += Time.deltaTime * (npcWalkSpeed / totalDistance);
            jaywalkingNPC.transform.position = Vector3.Lerp(npcStartPos, npcTargetPoint.position, walkProgress);

            // Spawn car around 30% progress
            if (walkProgress >= 0.3f && speedingCar != null && !speedingCar.activeSelf)
            {
                speedingCar.SetActive(true);
            }

            if (speedingCar != null && speedingCar.activeSelf && carCrashPoint != null)
            {
                speedingCar.transform.position = Vector3.MoveTowards(
                    speedingCar.transform.position,
                    carCrashPoint.position,
                    carSpeed * Time.deltaTime
                );

                if (Vector3.Distance(speedingCar.transform.position, carCrashPoint.position) < 0.8f)
                {
                    impactOccurred = true;
                }
            }

            yield return null;
        }

        // Trigger Crash FX & hide NPC
        if (fireAndSmokeFX != null && carCrashPoint != null)
        {
            fireAndSmokeFX.transform.position = carCrashPoint.position;
            fireAndSmokeFX.SetActive(true);
        }

        if (jaywalkingNPC != null) jaywalkingNPC.SetActive(false);

        yield return new WaitForSeconds(2.5f);

        if (floatingMiniDialogue != null) floatingMiniDialogue.gameObject.SetActive(false);
        if (policeDialogueScript != null) policeDialogueScript.ChooseWhy();
    }
}