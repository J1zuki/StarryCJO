/*
 * Author: Joyce Kwek
 * Date: 11th August 2026
 * File: ExitGameButton.cs
 * Description:
 * Allows the player to exit the built Unity application
 * by pressing the Exit Game button on the Completion UI.
 */

using UnityEngine;

/// <summary>
/// Controls the Exit Game button on the Completion UI.
/// In a built application, it closes the program.
/// In the Unity Editor, it stops Play Mode.
/// </summary>
public class ExitGameButton : MonoBehaviour
{
    /// <summary>
    /// Exits the built Unity application.
    /// When testing inside the Unity Editor,
    /// it stops Play Mode instead.
    /// </summary>
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}