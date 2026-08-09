using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class NPCAccidentSequence : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TrafficLightControl trafficLight;
    [SerializeField] private GameObject jaywalkingNPC;
    [SerializeField] private Transform npcTargetPoint;
    [SerializeField] private GameObject speedingCar;
    [SerializeField] private Transform carCrashPoint;

    [Header("Fire & Smoke FX")]
    [SerializeField] private GameObject fireAndSmokeFX;

    [Header("Dialogue & UI")]
    [SerializeField] private TMP_Text floatingMiniDialogue; 
    [SerializeField] private PoliceOfficerInteraction policeDialogueScript;

    [Header("Settings")]
    [SerializeField] private float npcWalkSpeed = 2f;
    [SerializeField] private float carSpeed = 25f;

    private NavMeshAgent npcAgent;
    private bool sequenceTriggered = false;

    private void Start()
    {
        if (fireAndSmokeFX != null) fireAndSmokeFX.SetActive(false);
        if (speedingCar != null) speedingCar.SetActive(false);

        if (jaywalkingNPC != null)
        {
            npcAgent = jaywalkingNPC.GetComponent<NavMeshAgent>();
            if (npcAgent != null) npcAgent.speed = npcWalkSpeed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (sequenceTriggered) return;

        if (other.CompareTag("Player"))
        {
            if (trafficLight != null && trafficLight.currentState == TrafficLightControl.LightState.Red)
            {
                sequenceTriggered = true;
                StartCoroutine(RunAccidentSequence());
            }
        }
    }

    private IEnumerator RunAccidentSequence()
    {
        if (floatingMiniDialogue != null)
        {
            floatingMiniDialogue.gameObject.SetActive(true);
            floatingMiniDialogue.text = "This is the wrong way of crossing... Now, let me show you the right way.";
        }

        // Command NavMeshAgent to move across
        if (npcAgent != null && npcTargetPoint != null)
        {
            npcAgent.SetDestination(npcTargetPoint.position);
        }

        bool carSpawned = false;
        bool impactOccurred = false;

        while (!impactOccurred)
        {
            // Spawn car when NPC reaches midway on the street
            if (!carSpawned && npcAgent != null && npcAgent.remainingDistance < 3f)
            {
                if (speedingCar != null)
                {
                    speedingCar.SetActive(true);
                    carSpawned = true;
                }
            }

            // Move car toward crash point
            if (carSpawned && speedingCar != null && carCrashPoint != null)
            {
                speedingCar.transform.position = Vector3.MoveTowards(
                    speedingCar.transform.position,
                    carCrashPoint.position,
                    carSpeed * Time.deltaTime
                );

                // Detect crash distance
                if (Vector3.Distance(speedingCar.transform.position, carCrashPoint.position) < 0.8f)
                {
                    impactOccurred = true;
                }
            }

            yield return null;
        }

        // Trigger Impact Effects
        if (fireAndSmokeFX != null && carCrashPoint != null)
        {
            fireAndSmokeFX.transform.position = carCrashPoint.position;
            fireAndSmokeFX.SetActive(true);
        }

        if (npcAgent != null) npcAgent.isStopped = true;
        if (jaywalkingNPC != null) jaywalkingNPC.SetActive(false);

        yield return new WaitForSeconds(2.5f);

        if (floatingMiniDialogue != null) floatingMiniDialogue.gameObject.SetActive(false);
        if (policeDialogueScript != null) policeDialogueScript.ChooseWhy();
    }
}