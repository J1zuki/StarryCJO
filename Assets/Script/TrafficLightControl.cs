using System.Collections;
using UnityEngine;

public class TrafficLightControl : MonoBehaviour
{
    public enum LightState { Red, Yellow, Green }
    public LightState currentState = LightState.Red;

    [Header("Visual Light Objects")]
    [SerializeField] private GameObject redLightObject;
    [SerializeField] private GameObject greenLightObject;

    [Header("Paired Light Instance")]
    [SerializeField] private TrafficLightControl oppositeTrafficLight;

    [Header("Timing & Audio")]
    [SerializeField] private float delayBeforeGreen = 2f;
    [SerializeField] private float greenDuration = 6f;
    [SerializeField] private AudioClip buttonClickSFX;
    [SerializeField] private AudioClip greenBeepSFX;

    [Header("Safe Pedestrian Demo")]
    [SerializeField] private SafeNPCController safeNPC; 

    private bool buttonPressed = false;

    private void Start()
    {
        SetLightState(LightState.Red);
    }

    public void InteractWithButton()
    {
        if (buttonPressed) return;
        
        buttonPressed = true;
        if (oppositeTrafficLight != null) oppositeTrafficLight.buttonPressed = true;

        if (buttonClickSFX != null) AudioSource.PlayClipAtPoint(buttonClickSFX, transform.position);

        StartCoroutine(TrafficLightSequence());
    }

    private IEnumerator TrafficLightSequence()
    {
        yield return new WaitForSeconds(delayBeforeGreen);

        // Set Green on both lights
        SetLightState(LightState.Green);
        if (oppositeTrafficLight != null) oppositeTrafficLight.SetLightState(LightState.Green);

        if (greenBeepSFX != null) AudioSource.PlayClipAtPoint(greenBeepSFX, transform.position);

        // Start Safe NPC NavMesh movement
        if (safeNPC != null) safeNPC.StartSafeCrossing();

        yield return new WaitForSeconds(greenDuration);

        // Reset to Red on both lights
        SetLightState(LightState.Red);
        if (oppositeTrafficLight != null) oppositeTrafficLight.SetLightState(LightState.Red);

        buttonPressed = false;
        if (oppositeTrafficLight != null) oppositeTrafficLight.buttonPressed = false;
    }

    public void SetLightState(LightState state)
    {
        currentState = state;
        if (redLightObject != null) redLightObject.SetActive(state == LightState.Red);
        if (greenLightObject != null) greenLightObject.SetActive(state == LightState.Green);
    }
}