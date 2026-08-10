/*
 * Author: Joyce Kwek
 * Date: 10th August 2026
 * File: TrafficLightControl.cs
 * Description:
 * Controls the pedestrian traffic light system.
 * It synchronises two opposite traffic lights, plays sound effects,
 * changes between red and green states, and can start the safe NPC crossing.
 */

using System.Collections;
using UnityEngine;

/// <summary>
/// Controls a pedestrian traffic light and keeps it synchronised
/// with the traffic light on the opposite side of the road.
/// </summary>
public class TrafficLightControl : MonoBehaviour
{
    /// <summary>
    /// Possible states for the pedestrian traffic light.
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

    [Header("Paired Light Instance")]
    [SerializeField] private TrafficLightControl oppositeTrafficLight;

    [Header("Timing")]
    [SerializeField] private float delayBeforeGreen = 2f;
    [SerializeField] private float greenDuration = 6f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip buttonClickSFX;
    [SerializeField] private AudioClip greenBeepSFX;

    [Header("Safe Pedestrian Demo")]
    [SerializeField] private SafeNPCController safeNPC;

    private bool buttonPressed;

    /// <summary>
    /// Returns the current traffic-light state.
    /// This is used by other scripts such as WrongNPCController.
    /// </summary>
    public LightState CurrentState => currentState;

    /// <summary>
    /// Returns whether the pedestrian crossing button
    /// has already been pressed.
    /// </summary>
    public bool ButtonPressed => buttonPressed;

    /// <summary>
    /// Sets the pedestrian light to red when the scene begins.
    /// </summary>
    private void Start()
    {
        SetLightState(LightState.Red);
    }

    /// <summary>
    /// Starts the pedestrian crossing sequence when
    /// the crossing button is activated.
    /// </summary>
    public void InteractWithButton()
    {
        if (buttonPressed)
            return;

        buttonPressed = true;

        if (oppositeTrafficLight != null)
            oppositeTrafficLight.SetButtonPressed(true);

        PlaySound(buttonClickSFX);

        StartCoroutine(TrafficLightSequence());
    }

    /// <summary>
    /// Waits before changing both pedestrian signals to green,
    /// starts the safe NPC crossing, and resets both lights to red.
    /// </summary>
    private IEnumerator TrafficLightSequence()
    {
        SetBothLights(LightState.Red);

        yield return new WaitForSeconds(delayBeforeGreen);

        SetBothLights(LightState.Green);

        PlaySound(greenBeepSFX);

        if (safeNPC != null)
            safeNPC.StartSafeCrossing();

        yield return new WaitForSeconds(greenDuration);

        SetBothLights(LightState.Red);

        buttonPressed = false;

        if (oppositeTrafficLight != null)
            oppositeTrafficLight.SetButtonPressed(false);
    }

    /// <summary>
    /// Changes both this traffic light and the opposite
    /// traffic light to the same state.
    /// </summary>
    /// <param name="state">
    /// The new pedestrian traffic-light state.
    /// </param>
    public void SetBothLights(LightState state)
    {
        SetLightState(state);

        if (oppositeTrafficLight != null)
            oppositeTrafficLight.SetLightState(state);
    }

    /// <summary>
    /// Changes this traffic light's visual state.
    /// </summary>
    /// <param name="state">
    /// The new pedestrian traffic-light state.
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
    /// Updates the crossing-button state for this traffic light.
    /// Used to keep both paired traffic lights synchronised.
    /// </summary>
    /// <param name="pressed">
    /// Whether the crossing button is currently active.
    /// </param>
    private void SetButtonPressed(bool pressed)
    {
        buttonPressed = pressed;
    }

    /// <summary>
    /// Plays the specified sound through the assigned AudioSource.
    /// </summary>
    /// <param name="clip">
    /// The AudioClip that should be played.
    /// </param>
    private void PlaySound(AudioClip clip)
    {
        if (audioSource == null || clip == null)
            return;

        audioSource.PlayOneShot(clip);
    }
}