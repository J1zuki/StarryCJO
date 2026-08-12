/*
 * Author: Cylina Ho
 * Date: 12th August 2026
 * File: InstructionUI.cs
 * Description:
 * Controls the display of the Instruction Screen UI.
 * Hides the instruction panel while the Home Screen,
 * Game Over panel, or Completion panel is active.
 */

using UnityEngine;

public class InstructionUI : MonoBehaviour
{
    [Header("Instruction Panel UI")]
    [SerializeField] private GameObject instructionPanel;

    [Header("Panels That Hide Instructions")]
    [SerializeField] private GameObject[] panelsToHideFor;

    private void Start()
    {
        if (instructionPanel == null)
        {
            Debug.LogWarning(
                "InstructionUI: Instruction Panel has not been assigned.",
                gameObject
            );
        }
    }

    private void Update()
    {
        if (instructionPanel == null)
        {
            return;
        }

        if (IsAnyPanelActive())
        {
            instructionPanel.SetActive(false);
        }
        else
        {
            instructionPanel.SetActive(true);
        }
    }

    private bool IsAnyPanelActive()
    {
        if (panelsToHideFor == null)
        {
            return false;
        }

        foreach (GameObject panel in panelsToHideFor)
        {
            if (panel != null && panel.activeInHierarchy)
            {
                return true;
            }
        }

        return false;
    }
}