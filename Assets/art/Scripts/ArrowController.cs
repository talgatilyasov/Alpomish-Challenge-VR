using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [SerializeField]
    private GameObject midPointVisual;

    [SerializeField]
    private GameObject arrowPrefab;

    [SerializeField]
    private GameObject arrowSpawnPoint;

    [SerializeField]
    private float arrowMaxSpeed = 10f;

    public void PrepareArrow()
    {
        midPointVisual.SetActive(true);
    }

    public void ReleaseArrow(float strength)
    {
        midPointVisual.SetActive(false);

        GameObject arrow = Instantiate(
            arrowPrefab,
            arrowSpawnPoint.transform.position,
            arrowSpawnPoint.transform.rotation
        );

        Rigidbody rb = arrow.GetComponent<Rigidbody>();

        rb.AddForce(
            arrowSpawnPoint.transform.forward * strength * arrowMaxSpeed,
            ForceMode.Impulse
        );
    }
}