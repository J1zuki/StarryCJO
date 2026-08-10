/*
 * Author: Joyce Kwek
 * Date: 10th August 2026
 * File: TrafficLightControl.cs
 * Description:
 * Controls the pedestrian traffic signal.
 * Two traffic-light poles can be paired so that both signals
 * always display the same red or green state.
 * Pressing B requests the pedestrian crossing sequence.
 */

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

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
    [SerializeField] private LightState currentState = LightState.Red;

    [Header("Visual Light Objects")]
    [SerializeField] private GameObject redLightObject;
    [SerializeField] private GameObject greenLightObject;

    [Header("Paired Traffic Light")]
    [SerializeField] private TrafficLightControl oppositeTrafficLight;

    [Header("Button Interaction")]
    [SerializeField] private bool allowBKeyInteraction = true;

    [Header("Timing")]
    [SerializeField] private float delayBeforeGreen = 2f;
    [SerializeField] private float greenDuration = 6f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip buttonClickSFX;
    [SerializeField] private AudioClip greenBeepSFX;

    private bool buttonPressed;
    private Coroutine trafficLightCoroutine;

    /// <summary>
    /// Returns the current pedestrian traffic-light state.
    /// NPCGirl can use this to check whether the signal is red.
    /// </summary>
    public LightState CurrentState => currentState;

    /// <summary>
    /// Returns whether the crossing button has already
    /// been pressed.
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
    /// Detects the B key and starts the pedestrian
    /// crossing sequence when B interaction is enabled.
    /// </summary>
    private void Update()
    {
        if (!allowBKeyInteraction)
            return;

        if (Keyboard.current == null)
            return;

        if (Keyboard.current.bKey.wasPressedThisFrame)
        {
            InteractWithButton();
        }
    }

    /// <summary>
    /// Handles the pedestrian crossing request.
    /// It synchronises both traffic lights, plays the
    /// button sound, and begins the light sequence.
    /// </summary>
    public void InteractWithButton()
    {
        if (buttonPressed)
            return;

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

        trafficLightCoroutine =
            StartCoroutine(TrafficLightSequence());
    }

    /// <summary>
    /// Keeps both pedestrian signals red for a short delay,
    /// changes both lights to green, waits for the green duration,
    /// and then changes both lights back to red.
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
    /// Changes this traffic light and the paired
    /// traffic light to the same state.
    /// </summary>
    /// <param name="state">
    /// The new pedestrian traffic-light state.
    /// </param>
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
    /// <param name="state">
    /// The new traffic-light state.
    /// </param>
    public void SetLightState(LightState state)
    {
        currentState = state;

        if (redLightObject != null)
        {
            redLightObject.SetActive(
                state == LightState.Red
            );
        }

        if (greenLightObject != null)
        {
            greenLightObject.SetActive(
                state == LightState.Green
            );
        }
    }

    /// <summary>
    /// Updates the internal crossing-button state.
    /// This is used to keep the paired traffic light
    /// synchronised with the main traffic light.
    /// </summary>
    /// <param name="pressed">
    /// Whether the crossing button is currently active.
    /// </param>
    private void SetButtonPressed(bool pressed)
    {
        buttonPressed = pressed;
    }

    /// <summary>
    /// Plays the specified traffic-light sound
    /// through the assigned AudioSource.
    /// </summary>
    /// <param name="audioClip">
    /// Audio clip that should be played.
    /// </param>
    private void PlaySound(AudioClip audioClip)
    {
        if (audioSource == null || audioClip == null)
            return;

        audioSource.PlayOneShot(audioClip);
    }
}