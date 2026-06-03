using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowRotation : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private float rotateSpeed = 15f;

    private void FixedUpdate()
    {
        if (rb.velocity.magnitude > 0.1f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(rb.velocity.normalized);

            transform.rotation =
                Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    rotateSpeed * Time.fixedDeltaTime
                );
        }
    }
}