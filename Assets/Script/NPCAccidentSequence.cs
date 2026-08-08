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

    [Header("Dialogue & UI")]
    [SerializeField] private TMP_Text floatingMiniDialogue; // Shows "This is wrong" above NPC
    [SerializeField] private PoliceOfficerInteraction policeDialogueScript;

    [Header("Settings")]
    [SerializeField] private float npcWalkSpeed = 2f;
    [SerializeField] private float carSpeed = 25f;

    private bool sequenceTriggered = false;

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
        // 1. NPC begins jaywalking across the road
        if (floatingMiniDialogue != null)
        {
            floatingMiniDialogue.gameObject.SetActive(true);
            floatingMiniDialogue.text = "This is wrong...";
        }

        float walkProgress = 0f;
        Vector3 npcStartPos = jaywalkingNPC.transform.position;

        while (walkProgress < 1f)
        {
            walkProgress += Time.deltaTime * (npcWalkSpeed / Vector3.Distance(npcStartPos, npcTargetPoint.position));
            jaywalkingNPC.transform.position = Vector3.Lerp(npcStartPos, npcTargetPoint.position, walkProgress);

            // Halfway across, spawn/drive the car
            if (walkProgress >= 0.4f && speedingCar != null && !speedingCar.activeSelf)
            {
                speedingCar.SetActive(true);
            }

            // Move car toward crash point
            if (speedingCar != null && speedingCar.activeSelf)
            {
                speedingCar.transform.position = Vector3.MoveTowards(
                    speedingCar.transform.position,
                    carCrashPoint.position,
                    carSpeed * Time.deltaTime
                );
            }

            yield return null;
        }

        // 2. Hide Mini Dialogue
        if (floatingMiniDialogue != null)
        {
            floatingMiniDialogue.gameObject.SetActive(false);
        }

        // 3. Trigger your Police Officer interaction script dialogue automatically
        if (policeDialogueScript != null)
        {
            // Police officer intervenes with dialogue: "This is why we must stay alert!"
            policeDialogueScript.ChooseWhy(); // Displays explanation dialogue
        }
    }
}