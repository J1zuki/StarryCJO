using System.Collections;
using TMPro;
using UnityEngine;

public class SafeNPCController : MonoBehaviour
{
    [Header("Movement Points")]
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform targetPoint;

    [Header("Dialogue & Settings")]
    [SerializeField] private TMP_Text miniDialogue;
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private Transform npcHead; // Optional: Assign head bone to show looking left/right

    private bool hasCrossed = false;

    private void Start()
    {
        if (startPoint != null) transform.position = startPoint.position;
    }

    public void StartSafeCrossing()
    {
        if (hasCrossed) return;
        hasCrossed = true;
        StartCoroutine(RunSafeCrossingSequence());
    }

    private IEnumerator RunSafeCrossingSequence()
    {
        // 1. Show message & look left and right before stepping onto the road
        if (miniDialogue != null)
        {
            miniDialogue.gameObject.SetActive(true);
            miniDialogue.text = "Light is green! Looking left and right before crossing...";
        }

        // Look Left
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y - 45f, 0);
        yield return new WaitForSeconds(0.8f);

        // Look Right
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y + 90f, 0);
        yield return new WaitForSeconds(0.8f);

        // Face Target Point
        transform.LookAt(targetPoint);
        yield return new WaitForSeconds(0.4f);

        if (miniDialogue != null)
        {
            miniDialogue.text = "Crossing safely!";
        }

        // 2. Walk safely across the crosswalk
        float walkProgress = 0f;
        Vector3 startPos = transform.position;
        float distance = Vector3.Distance(startPos, targetPoint.position);

        while (walkProgress < 1f)
        {
            walkProgress += Time.deltaTime * (walkSpeed / distance);
            transform.position = Vector3.Lerp(startPos, targetPoint.position, walkProgress);
            yield return null;
        }

        // 3. Clear dialogue
        yield return new WaitForSeconds(1.5f);
        if (miniDialogue != null)
        {
            miniDialogue.gameObject.SetActive(false);
        }
    }
}