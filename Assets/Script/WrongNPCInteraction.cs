using UnityEngine;

public class WrongNPCInteraction : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private NPCAccidentSequence accidentSequenceScript;

    public void OnPlayerTalk()
    {
        // Trigger the accident sequence coroutine
        if (accidentSequenceScript != null)
        {
            accidentSequenceScript.StartJaywalkingSequence();
        }
    }
}