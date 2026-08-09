using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractionRaycast : MonoBehaviour
{
    [SerializeField] private float rayDistance = 3f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private GameObject interactionUI;

    // Assign this via the Inspector or set up in Input System
    [SerializeField] private InputAction interactAction; 

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
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
            TrafficLightControl trafficLight = hit.collider.GetComponentInParent<TrafficLightControl>();

            if (trafficLight != null)
            {
                if (interactionUI != null) interactionUI.SetActive(true);

                // Triggers whenever your configured "Interact" action is pressed
                if (interactAction.WasPressedThisFrame())
                {
                    trafficLight.InteractWithButton();
                }
                return;
            }
        }

        if (interactionUI != null) interactionUI.SetActive(false);
    }
}