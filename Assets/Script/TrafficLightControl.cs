using UnityEngine;
using System.Collections;

public class TrafficLightControl : MonoBehaviour
{
    public enum LightState { Red, Yellow, Green }

    [Header("Current State")]
    public LightState currentState = LightState.Red;

    [Header("Visual Light Objects / Materials")]
    [SerializeField] private GameObject redLightObject;
    [SerializeField] private GameObject greenLightObject;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip buttonClickSFX;
    [SerializeField] private AudioClip greenBeepSFX;

    [Header("Timing")]
    [SerializeField] private float delayBeforeGreen = 3f;
    [SerializeField] private float greenDuration = 8f;

    [Header("Safe Pedestrian Demo")]
    [SerializeField] private SafeNPCController safeNPC; 

    private bool buttonPressed = false;

    private void Start()
    {
        SetLightState(LightState.Red);
    }

    /// <summary>
    /// Called when the player interacts with the button via Raycast.
    /// </summary>
    public void InteractWithButton()
    {
        if (buttonPressed) return; // Prevent double pressing

        buttonPressed = true;

        // Play Button Click Sound at the traffic light's position
        if (buttonClickSFX != null)
        {
            AudioSource.PlayClipAtPoint(buttonClickSFX, transform.position);
        }

        // Start cycle to change light
        StartCoroutine(TrafficLightSequence());
    }

    private IEnumerator TrafficLightSequence()
    {
        yield return new WaitForSeconds(delayBeforeGreen);

        // Turn Green
        SetLightState(LightState.Green);

        // Play green beep sound at the traffic light's position
        if (greenBeepSFX != null)
        {
            AudioSource.PlayClipAtPoint(greenBeepSFX, transform.position);
        }

        // Trigger safe NPC to walk across now that light is green
        if (safeNPC != null)
        {
            safeNPC.StartSafeCrossing();
        }

        yield return new WaitForSeconds(greenDuration);

        // Turn Red
        SetLightState(LightState.Red);
        buttonPressed = false;
    }

    private void SetLightState(LightState state)
    {
        currentState = state;

        if (redLightObject != null) redLightObject.SetActive(state == LightState.Red);
        if (greenLightObject != null) greenLightObject.SetActive(state == LightState.Green);
    }
}