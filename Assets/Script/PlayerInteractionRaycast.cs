using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerInteractionRaycast : MonoBehaviour
{
    [SerializeField] private float rayDistance = 3f;
    [SerializeField] private LayerMask interactableLayer;

    [Header("UI Reference")]
    [SerializeField] private GameObject interactionUI; // The black banner panel at the top
    [SerializeField] private TMP_Text interactionText;  // TextMeshPro component inside the panel

    [Header("Input Setup")]
    [SerializeField] private InputAction interactAction;

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
        // Optional fallback if interactAction isn't set via Inspector:
        if (interactAction == null || interactAction.bindings.Count == 0)
        {
            interactAction = new UnityEngine.InputSystem.InputAction("Interact", binding: "<Keyboard>/b");
        }
    }

    private void OnEnable()
    {
        interactAction.Enable();
    }

    private void OnDisable()
    {
        interactAction.Disable();
    }

    private void Update()
    {
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance, interactableLayer))
        {
            // 1. Check for Traffic Light Button
            TrafficLightControl trafficLight = hit.collider.GetComponentInParent<TrafficLightControl>();
            if (trafficLight != null)
            {
                ShowPrompt("Press 'B' to press traffic button");

                if (interactAction.WasPressedThisFrame())
                {
                    trafficLight.InteractWithButton();
                }
                return;
            }

            // 2. Check for Police Officer / Dialogue NPC
            PoliceOfficerInteraction policeScript = hit.collider.GetComponentInParent<PoliceOfficerInteraction>();
            if (policeScript != null)
            {
                ShowPrompt("Press 'B' to talk to police officer");

                if (interactAction.WasPressedThisFrame())
                {
                    policeScript.ChooseWhy();
                }
                return;
            }
        }

        // Hide prompt if looking away from interactables
        HidePrompt();
    }

    private void ShowPrompt(string message)
    {
        if (interactionUI != null) interactionUI.SetActive(true);
        if (interactionText != null) interactionText.text = message;
    }

    private void HidePrompt()
    {
        if (interactionUI != null) interactionUI.SetActive(false);
    }
}