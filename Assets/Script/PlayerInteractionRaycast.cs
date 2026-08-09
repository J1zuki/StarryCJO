using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    private Camera mainCamera;
    private bool panelActive = false;

    private void Awake()
    {
        mainCamera = Camera.main;
        if (closeButton != null) closeButton.onClick.AddListener(HidePanel);
        HidePanel();
    }

    private void Update()
    {
        if (panelActive) return; // Stop raycasting while UI is open

        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, interactableLayer))
        {
            // Traffic Light Interaction
            TrafficLightControl trafficLight = hit.collider.GetComponentInParent<TrafficLightControl>();
            if (trafficLight != null)
            {
                ShowPanel("Interact with Traffic Light?", () => {
                    trafficLight.InteractWithButton();
                    HidePanel();
                });
                return;
            }

            // Police Officer Interaction
            PoliceOfficerInteraction policeScript = hit.collider.GetComponentInParent<PoliceOfficerInteraction>();
            if (policeScript != null)
            {
                ShowPanel("Talk to Police Officer?", () => {
                    policeScript.ChooseWhy();
                    HidePanel();
                });
                return;
            }
        }
    }

    private void ShowPanel(string message, UnityEngine.Events.UnityAction action)
    {
        panelActive = true;
        if (interactionPanel != null) interactionPanel.SetActive(true);
        if (promptText != null) promptText.text = message;

        if (actionButton != null)
        {
            actionButton.onClick.RemoveAllListeners();
            actionButton.onClick.AddListener(action);
        }

        // Unlock cursor for UI clicking
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void HidePanel()
    {
        panelActive = false;
        if (interactionPanel != null) interactionPanel.SetActive(false);

        // Lock cursor back for gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}