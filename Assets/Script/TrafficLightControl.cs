using UnityEngine;
using System.Collections;

public class TrafficLightControl : MonoBehaviour
{
    public enum LightState { Red, Yellow, Green }
    public LightState currentState = LightState.Red;

    [Header("Visual Light Objects")]
    [SerializeField] private GameObject redLightObject;
    [SerializeField] private GameObject greenLightObject;

    [Header("Paired Light Instance")]
    [SerializeField] private TrafficLightControl oppositeTrafficLight;

    [Header("Settings & Audio")]
    [SerializeField] private AudioClip buttonClickSFX;
    [SerializeField] private AudioClip greenBeepSFX;
    [SerializeField] private float delayBeforeGreen = 3f;
    [SerializeField] private float greenDuration = 8f;
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

        SetLightState(LightState.Green);
        if (oppositeTrafficLight != null) oppositeTrafficLight.SetLightState(LightState.Green);

        if (greenBeepSFX != null) AudioSource.PlayClipAtPoint(greenBeepSFX, transform.position);

        if (safeNPC != null) safeNPC.StartSafeCrossing();

        yield return new WaitForSeconds(greenDuration);

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