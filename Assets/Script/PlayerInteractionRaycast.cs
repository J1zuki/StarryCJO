using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractionRaycast : MonoBehaviour
{
    [SerializeField] private float rayDistance = 3f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private GameObject interactionUI; // "Press E to Push Button" UI

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance, interactableLayer))
        {
            TrafficLightController trafficLight = hit.collider.GetComponentInParent<TrafficLightController>();

            if (trafficLight != null)
            {
                if (interactionUI != null) interactionUI.SetActive(true);

                if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
                {
                    trafficLight.InteractWithButton();
                }
                return;
            }
        }

        if (interactionUI != null) interactionUI.SetActive(false);
    }
}