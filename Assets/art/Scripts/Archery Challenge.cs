using UnityEngine;

public class ArcheryChallenge : MonoBehaviour
{
    [Header("Target Waves")]
    public GameObject[] wave1Targets;
    public GameObject[] wave2Targets;
    public GameObject[] wave3Targets;

    [Header("Next Canvas")]
    public GameObject nextCanvas;

    private int currentWave = 1;

    void Start()
    {
        ActivateWave(wave1Targets, true);
        ActivateWave(wave2Targets, false);
        ActivateWave(wave3Targets, false);

        // Canvas выключен в начале
        if (nextCanvas != null)
        {
            nextCanvas.SetActive(false);
        }
    }

    // Вызывается мишенью при попадании
    public void CheckWaveComplete()
    {
        if (currentWave == 1)
        {
            if (AllTargetsDown(wave1Targets))
            {
                ActivateWave(wave2Targets, true);

                currentWave = 2;
            }
        }
        else if (currentWave == 2)
        {
            if (AllTargetsDown(wave2Targets))
            {
                ActivateWave(wave3Targets, true);

                currentWave = 3;
            }
        }
        else if (currentWave == 3)
        {
            if (AllTargetsDown(wave3Targets))
            {
                Debug.Log("ALL WAVES COMPLETE");

                // Включить canvas
                if (nextCanvas != null)
                {
                    nextCanvas.SetActive(true);
                }
            }
        }
    }

    bool AllTargetsDown(GameObject[] targets)
    {
        foreach (GameObject target in targets)
        {
            TargetFall tf = target.GetComponent<TargetFall>();

            if (tf != null && !tf.isHit)
            {
                return false;
            }
        }

        return true;
    }

    void ActivateWave(GameObject[] targets, bool state)
    {
        foreach (GameObject target in targets)
        {
            target.SetActive(state);
        }
    }
}