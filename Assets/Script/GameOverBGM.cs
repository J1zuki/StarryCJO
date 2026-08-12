/*
 * Author: Olivia Chai
 * Date: 12th August 2026
 * File: GameOverBGM.cs
 * Description:
 * Controls the background music for the game over screen.
 */

using UnityEngine;

public class GameOverBGM : MonoBehaviour
{
    public AudioSource mainBGM;
    public AudioSource gameOverBGM;
    /// <summary>
    /// Called automatically when the Game Over panel becomes active.
    /// Pauses the normal BGM and plays the Game Over BGM.
    /// </summary>
    void OnEnable()
    {
        if (mainBGM != null)
        {
            mainBGM.Pause();
        }

        if (gameOverBGM != null)
        {
            gameOverBGM.Play();
        }
    }

    /// <summary>
    /// Called automatically when the Game Over panel becomes inactive.
    /// Stops the Game Over BGM and resumes the normal BGM.
    /// </summary>
    void OnDisable()
    {
        if (gameOverBGM != null)
        {
            gameOverBGM.Stop();
        }

        if (mainBGM != null)
        {
            mainBGM.UnPause();
        }
    }
}
