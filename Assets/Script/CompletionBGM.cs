/*
 * Author: Olivia Chai
 * Date: 12th August 2026
 * File: CompletionBGM.cs
 * Description:
 * Controls the background music for the game completion screen.
 */
using UnityEngine;

public class CompletionBGM : MonoBehaviour
{
    public AudioSource mainBGM;
    public AudioSource completionBGM;

    /// <summary>
    /// Called automatically when the Completion panel becomes active.
    /// </summary>
    void OnEnable()
    {
        if (mainBGM != null)
        {
            mainBGM.Pause();
        }

        if (completionBGM != null)
        {
            completionBGM.Play();
        }
    }
    /// <summary>
    /// Called automatically when the Completion panel becomes inactive.
    /// </summary>
    void OnDisable()
    {
        if (completionBGM != null)
        {
            completionBGM.Stop();
        }

        if (mainBGM != null)
        {
            mainBGM.UnPause();
        }
    }


}
