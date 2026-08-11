/*
 * Author: Joyce Kwek
 * Date: 11th August 2026
 * File: TrafficLightControl.cs
 * Description:
 * Controls the pedestrian traffic signal.
 * Two traffic-light poles can be paired so that both signals
 * always display the same red or green state.
 * The crossing sequence is activated through a traffic pole
 * interaction trigger.
 */

using System.Collections;
using UnityEngine;

/// <summary>
/// Controls a pedestrian traffic light and synchronises its
/// signal state with a paired traffic light on the opposite
/// side of the road.
/// </summary>
public class TrafficLightControl : MonoBehaviour
{
    /// <summary>
    /// Available pedestrian traffic-light states.
    /// </summary>
    public enum LightState
    {
        Red,
        Yellow,
        Green
    }

    [Header("Current State")]
    [SerializeField]
    private LightState currentState = LightState.Red;

    [Header("Visual Light Objects")]
    [SerializeField] private GameObject redLightObject;
    [SerializeField] private GameObject greenLightObject;

    [Header("Paired Traffic Light")]
    [SerializeField]
    private TrafficLightControl oppositeTrafficLight;

    [Header("Timing")]
    [SerializeField] private float delayBeforeGreen = 2f;
    [SerializeField] private float greenDuration = 6f;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip buttonClickSFX;
    [SerializeField] private AudioClip greenBeepSFX;

    private bool buttonPressed;
    private Coroutine trafficLightCoroutine;

    /// <summary>
    /// Returns the current pedestrian
    /// traffic-light state.
    /// </summary>
    public LightState CurrentState => currentState;

    /// <summary>
    /// Returns whether the crossing button
    /// has already been pressed.
    /// </summary>
    public bool ButtonPressed => buttonPressed;

    /// <summary>
    /// Sets the pedestrian traffic light to red
    /// when the scene begins.
    /// </summary>
    private void Start()
    {
        SetLightState(LightState.Red);
    }

    /// <summary>
    /// Handles the pedestrian crossing request.
    /// This method is called by TrafficPoleInteraction
    /// when the player presses B at the traffic pole.
    /// </summary>
    public void InteractWithButton()
    {
        if (buttonPressed)
        {
            return;
        }

        buttonPressed = true;

        if (oppositeTrafficLight != null)
        {
            oppositeTrafficLight.SetButtonPressed(true);
        }

        PlaySound(buttonClickSFX);

        if (trafficLightCoroutine != null)
        {
            StopCoroutine(trafficLightCoroutine);
        }

        trafficLightCoroutine = StartCoroutine(TrafficLightSequence());
    }

    /// <summary>
    /// Keeps both pedestrian signals red,
    /// waits, changes them to green,
    /// then returns them to red.
    /// </summary>
    private IEnumerator TrafficLightSequence()
    {
        SetBothLights(LightState.Red);

        yield return new WaitForSeconds(delayBeforeGreen);

        SetBothLights(LightState.Green);

        PlaySound(greenBeepSFX);

        yield return new WaitForSeconds(greenDuration);

        SetBothLights(LightState.Red);

        buttonPressed = false;

        if (oppositeTrafficLight != null)
        {
            oppositeTrafficLight.SetButtonPressed(false);
        }

        trafficLightCoroutine = null;
    }

    /// <summary>
    /// Changes this traffic light and its paired
    /// traffic light to the same state.
    /// </summary>
    public void SetBothLights(LightState state)
    {
        SetLightState(state);

        if (oppositeTrafficLight != null)
        {
            oppositeTrafficLight.SetLightState(state);
        }
    }

    /// <summary>
    /// Changes this individual traffic light's
    /// red and green visual objects.
    /// </summary>
    public void SetLightState(LightState state)
    {
        currentState = state;

        if (redLightObject != null)
        {
            redLightObject.SetActive(state == LightState.Red);
        }

        if (greenLightObject != null)
        {
            greenLightObject.SetActive(state == LightState.Green);
        }
    }

    /// <summary>
    /// Updates the internal crossing-button state
    /// for the paired traffic light.
    /// </summary>
    private void SetButtonPressed(bool pressed)
    {
        buttonPressed = pressed;
    }

    /// <summary>
    /// Plays an audio clip at the position of this GameObject
    /// without requiring an AudioSource component.
    /// </summary>
    private void PlaySound(AudioClip audioClip)
    {
        if (audioClip != null)
        {
            AudioSource.PlayClipAtPoint(audioClip, transform.position);
        }
    }
}