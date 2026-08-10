using UnityEngine;

public class StoreZoneTrigger : MonoBehaviour
{
    [SerializeField] private StoreOwnerNPC storeOwner;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            storeOwner.OnPlayerEnterZone(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            storeOwner.OnPlayerExitZone();
        }
    }
}