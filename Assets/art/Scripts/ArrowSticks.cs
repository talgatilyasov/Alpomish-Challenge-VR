using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowStick : MonoBehaviour
{
    private Rigidbody rb;

    private bool stuck = false;

    [Header("Audio")]
    public AudioClip spawnClip;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        if (spawnClip != null)
        {
            AudioSource.PlayClipAtPoint(
                spawnClip,
                transform.position,
                1f
            );
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (stuck) return;

        stuck = true;

        // Останавливаем физику
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.isKinematic = true;

        // Отключаем collider стрелы
        Collider col = GetComponentInChildren<Collider>();

        if (col != null)
        {
            col.enabled = false;
        }

        // Приклеиваем к объекту
        transform.SetParent(collision.transform);
    }
}