
/*
 * Author: Olivia Chai
 * Date: 12th August 2026
 * File: HomeScreenBGM.cs
 * Description:
 * Controls the background music for the home screen.
 */

using UnityEngine;

public class HomeScreenBGM : MonoBehaviour
{
    public AudioSource homeScreenBGM;

    void OnEnable()
    {
        if (homeScreenBGM != null && !homeScreenBGM.isPlaying)
        {
            homeScreenBGM.Play();
        }
    }

    void OnDisable()
    {
        if (homeScreenBGM != null)
        {
            homeScreenBGM.Stop();
        }
    }
}
