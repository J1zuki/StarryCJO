/*
 * Author: Cylina ho
 * Date: 12th August 2026
 * File: InstructionUI.cs
 * Description:
 * Controls the display of the Instruction Screen UI.
 * Automatically hides the instructions if any game over, completion,
 * or end-game panel becomes active.
 */

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the instruction screen panel and automatically hides it
/// when end-game or completion panels are displayed.
/// </summary>

public class InstructionUI : MonoBehaviour
{
    [Header("Instruction Panel UI")]
    [SerializeField] private GameObject instructionPanel;

    [Header("End Game Panels to Watch")]
    [Tooltip("Assign panels like PoliceOfficerInteraction's gameOverPanel or CorrectCrossingMission's completionCanvas.")]
    [SerializeField] private GameObject[] panelsToHideFor;

    /// <summary>
    /// Verifies setup when the scene starts.
    /// </summary>
    private void Start()
    {
        if (instructionPanel == null)
        {
            Debug.LogWarning("InstructionUIController: Instruction Panel has not been assigned.", gameObject);
        }
    }

    /// <summary>
    /// Checks every frame if any end-game or completion panel is active,
    /// and toggles the instruction panel visibility accordingly.
    /// </summary>
    private void Update()
    {
        if (instructionPanel == null)
        {
            return;
        }

        // If any game over or end panel is open, hide the instruction screen
        if (IsAnyEndPanelActive())
        {
            if (instructionPanel.activeSelf)
            {
                instructionPanel.SetActive(false);
            }
        }
        else
        {
            // If no end panels are open, show the instruction screen
            if (!instructionPanel.activeSelf)
            {
                instructionPanel.SetActive(true);
            }
        }
    }

    /// <summary>
    /// Returns true if any assigned end panel is currently active in the scene hierarchy.
    /// </summary>
    /// <returns>True if an end panel is visible; otherwise false.</returns>
    private bool IsAnyEndPanelActive()
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