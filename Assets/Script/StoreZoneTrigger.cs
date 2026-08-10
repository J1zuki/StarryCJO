/*
 * Author: Cylina Ho & Joyce Kwek
 * Date: 10th August 2026
 * File: StoreZoneTrigger.cs
 * Description:
 * Detects when the player enters or exits the
 * Store Owner interaction zone.
 * When the player enters, the Store Owner is told
 * to move toward the player.
 */

using UnityEngine;

/// <summary>
/// Detects the player entering and exiting
/// the Store Owner interaction zone.
/// </summary>
public class StoreZoneTrigger : MonoBehaviour
{
    [Header("Store Owner Reference")]

    /// <summary>
    /// Store Owner controlled by this trigger.
    /// </summary>
    [SerializeField] private StoreOwnerNPC storeOwner;


    [Header("Player Settings")]

    /// <summary>
    /// Tag used to identify the player.
    /// </summary>
    [SerializeField] private string playerTag = "Player";


    /// <summary>
    /// Detects when the player enters
    /// the Store Owner trigger zone.
    /// </summary>
    /// <param name="other">
    /// Collider entering the trigger.
    /// </param>
    private void OnTriggerEnter(Collider other)
    {
        Transform player = FindPlayerTransform(other);

        if (player == null)
        {
            return;
        }

        if (storeOwner == null)
        {
            Debug.LogError(
                "StoreZoneTrigger: Store Owner has not been assigned."
            );

            return;
        }

        storeOwner.OnPlayerEnterZone(player);

        Debug.Log(
            "StoreZoneTrigger: Player detected."
        );
    }


    /// <summary>
    /// Detects when the player exits
    /// the Store Owner trigger zone.
    /// </summary>
    /// <param name="other">
    /// Collider leaving the trigger.
    /// </param>
    private void OnTriggerExit(Collider other)
    {
        Transform player = FindPlayerTransform(other);

        if (player == null)
        {
            return;
        }

        if (storeOwner != null)
        {
            storeOwner.OnPlayerExitZone();
        }

        Debug.Log(
            "StoreZoneTrigger: Player left the zone."
        );
    }


    /// <summary>
    /// Searches the entering collider and its parents
    /// for an object using the Player tag.
    /// </summary>
    /// <param name="other">
    /// Collider being checked.
    /// </param>
    /// <returns>
    /// Player Transform if found; otherwise null.
    /// </returns>
    private Transform FindPlayerTransform(Collider other)
    {
        Transform currentTransform = other.transform;

        while (currentTransform != null)
        {
            if (currentTransform.CompareTag(playerTag))
            {
                return currentTransform;
            }

            currentTransform = currentTransform.parent;
        }

        return null;
    }
}