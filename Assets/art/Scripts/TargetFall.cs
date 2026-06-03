using UnityEngine;
using System.Collections;

public class TargetFall : MonoBehaviour
{
    [Header("Settings")]
    public float fallAngle = 90f;
    public float fallSpeed = 3f;

    [Header("Test Hit")]
    public bool testHit = false;

    [HideInInspector]
    public bool isHit = false;

    private Quaternion startRotation;
    private Quaternion targetRotation;

    private ArcheryChallenge challengeManager;

    void Start()
    {
        startRotation = transform.rotation;

        targetRotation =
            Quaternion.Euler(
                transform.eulerAngles.x + fallAngle,
                transform.eulerAngles.y,
                transform.eulerAngles.z
            );

        challengeManager =
            FindObjectOfType<ArcheryChallenge>();
    }

    void Update()
    {
        // Тестовое попадание
        if (testHit && !isHit)
        {
            HitTarget();

            testHit = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isHit)
            return;

        if (collision.gameObject.CompareTag("Arrow"))
        {
            HitTarget();
        }
    }

    void HitTarget()
    {
        isHit = true;

        StartCoroutine(FallTarget());

        if (challengeManager != null)
        {
            challengeManager.CheckWaveComplete();
        }
    }

    IEnumerator FallTarget()
    {
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * fallSpeed;

            transform.rotation =
                Quaternion.Slerp(
                    startRotation,
                    targetRotation,
                    t
                );

            yield return null;
        }
    }
}