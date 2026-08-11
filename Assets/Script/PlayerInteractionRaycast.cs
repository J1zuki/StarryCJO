/*
 * Author: Joyce Kwek
 * Date: 10th August 2026
 * File: PlayerInteractionRaycast.cs
 * Description:
 * Uses a raycast from the player's camera to detect interactable
 * objects such as NPCGirl, traffic lights, and the police officer.
 */


using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;


/// <summary>
/// Detects interactable objects using a camera raycast
/// and allows the player to interact with them.
/// </summary>
public class PlayerInteractionRaycast : MonoBehaviour
{
    [Header("Raycast Setup")]
    [SerializeField] private float rayDistance = 4f;
    [SerializeField] private LayerMask interactableLayer;


    [Header("UI Panel Setup")]
    [SerializeField] private GameObject interactionPanel;
    [SerializeField] private TMP_Text promptText;
    [SerializeField] private Button actionButton;
    [SerializeField] private Button closeButton;


    [Header("Player Control")]
    [SerializeField] private Behaviour[] playerControlsToDisable;


    private Camera mainCamera;


    private bool panelActive;
    private UnityAction currentAction;


    /// <summary>
    /// Gets the main camera, prepares the close button,
    /// and hides the interaction panel when the game begins.
    /// </summary>
    private void Awake()
    {
        mainCamera = Camera.main;


        if (closeButton != null)
            closeButton.onClick.AddListener(HidePanel);


        HidePanel();
    }


    /// <summary>
    /// Casts a ray from the player's camera and checks
    /// whether the player is looking at an interactable object.
    /// </summary>
    private void Update()
    {
        if (panelActive)
            return;


        if (mainCamera == null)
            return;


        Ray ray = new Ray(
          mainCamera.transform.position,
          mainCamera.transform.forward
        );


        RaycastHit hit;


        if (!Physics.Raycast(
          ray,
          out hit,
          rayDistance,
          interactableLayer))
        {
            return;
        }


        // NPC Girl interaction
        NPCGirl npcGirl =
      hit.collider.GetComponentInParent<NPCGirl>();


        if (npcGirl != null)
        {
            if (Keyboard.current != null &&
              Keyboard.current.eKey.wasPressedThisFrame)
            {
                ShowPanel(
                  "Observe the pedestrian crossing on red?",
                  npcGirl.StartWrongCrossing
                );
            }


            return;
        }


        // Traffic light interaction
        TrafficLightControl trafficLight =
      hit.collider.GetComponentInParent<TrafficLightControl>();


        if (trafficLight != null)
        {
            if (Keyboard.current != null &&
              Keyboard.current.eKey.wasPressedThisFrame)
            {
                ShowPanel(
                  "Request pedestrian crossing?",
                  trafficLight.InteractWithButton
                );
            }


            return;
        }


        // Police officer interaction
        PoliceOfficerInteraction policeOfficer =
      hit.collider.GetComponentInParent<PoliceOfficerInteraction>();


        if (policeOfficer != null)
        {
            if (Keyboard.current != null &&
              Keyboard.current.eKey.wasPressedThisFrame)
            {
                ShowPanel(
                  "Talk to the police officer?",
                  policeOfficer.ChooseWhy
                );
            }
        }
    }


    /// <summary>
    /// Opens the interaction panel and stores the action
    /// that should run when the player clicks the action button.
    /// </summary>
    /// <param name="message">
    /// Message displayed in the interaction panel.
    /// </param>
    /// <param name="action">
    /// Action performed when the player confirms the interaction.
    /// </param>
    private void ShowPanel(
    string message,
    UnityAction action)
    {
        panelActive = true;
        currentAction = action;


        if (interactionPanel != null)
            interactionPanel.SetActive(true);


        if (promptText != null)
            promptText.text = message;


        if (actionButton != null)
        {
            actionButton.onClick.RemoveAllListeners();
            actionButton.onClick.AddListener(PerformCurrentAction);
        }


        DisablePlayerControls();


        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }


    /// <summary>
    /// Performs the stored interaction action
    /// and closes the interaction panel.
    /// </summary>
    private void PerformCurrentAction()
    {
        if (currentAction != null)
            currentAction.Invoke();


        HidePanel();
    }


    /// <summary>
    /// Hides the interaction panel and restores
    /// normal player controls.
    /// </summary>
    public void HidePanel()
    {
        panelActive = false;
        currentAction = null;


        if (interactionPanel != null)
            interactionPanel.SetActive(false);


        EnablePlayerControls();


        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    /// <summary>
    /// Temporarily disables the assigned player movement
    /// and camera-control scripts.
    /// </summary>
    private void DisablePlayerControls()
    {
        foreach (
          Behaviour playerControl
          in playerControlsToDisable)
        {
            if (playerControl != null)
                playerControl.enabled = false;
        }
    }


    /// <summary>
    /// Restores the assigned player movement
    /// and camera-control scripts.
    /// </summary>
    private void EnablePlayerControls()
    {
        foreach (
          Behaviour playerControl
          in playerControlsToDisable)
        {
            if (playerControl != null)
                playerControl.enabled = true;
        }
    }


    /// <summary>
    /// Removes the close-button listener when
    /// this component is destroyed.
    /// </summary>
    private void OnDestroy()
    {
        if (closeButton != null)
            closeButton.onClick.RemoveListener(HidePanel);
    }
}