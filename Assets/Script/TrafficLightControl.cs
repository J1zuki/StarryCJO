using UnityEngine;
using System.Collections;

public class TrafficLightControl : MonoBehaviour
{
    public enum LightState { Red, Yellow, Green }

    [Header("Current State")]
    public LightState currentState = LightState.Red;

    [Header("Visual Light Objects / Materials")]
    [SerializeField] private GameObject redLightObject;
    [SerializeField] private GameObject yellowLightObject;
    [SerializeField] private GameObject greenLightObject;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip buttonClickSFX;
    [SerializeField] private AudioClip greenBeepSFX;

    [Header("Timing")]
    [SerializeField] private float delayBeforeGreen = 3f;
    [SerializeField] private float greenDuration = 8f;

    private bool buttonPressed = false;

    private void Start()
    {
        SetLightState(LightState.Red);
    }

    /// <summary>
    /// Called when the player looks at the button and presses 'E' via Raycast.
    /// </summary>
    public void InteractWithButton()
    {
        if (buttonPressed) return; // Prevent double pressing

        buttonPressed = true;

        // Play Button Click Sound
        if (audioSource != null && buttonClickSFX != null)
        {
            audioSource.PlayOneShot(buttonClickSFX);
        }

        // Start cycle to change light
        StartCoroutine(TrafficLightSequence());
    }

    private IEnumerator TrafficLightSequence()
    {
        yield return new WaitForSeconds(delayBeforeGreen);

        // Turn Green
        SetLightState(LightState.Green);

        // Loop green beep sound while green
        if (audioSource != null && greenBeepSFX != null)
        {
            audioSource.clip = greenBeepSFX;
            audioSource.loop = true;
            audioSource.Play();
        }

        yield return new WaitForSeconds(greenDuration);

        // Stop Audio & Turn Red
        if (audioSource != null)
        {
            audioSource.Stop();
            audioSource.loop = false;
        }

        SetLightState(LightState.Red);
        buttonPressed = false;
    }

    private void SetLightState(LightState state)
    {
        currentState = state;

        if (redLightObject != null) redLightObject.SetActive(state == LightState.Red);
        if (yellowLightObject != null) yellowLightObject.SetActive(state == LightState.Yellow);
        if (greenLightObject != null) greenLightObject.SetActive(state == LightState.Green);
    }
}
