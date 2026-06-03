using UnityEngine;

public class FinishTrigger : MonoBehaviour
{
    public HorseRaceTimer raceTimer;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        raceTimer.FinishRace();
    }
}