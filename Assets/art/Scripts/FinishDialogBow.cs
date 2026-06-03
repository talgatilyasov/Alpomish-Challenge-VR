using UnityEngine;
using System.Collections;

public class DelayedTeleport : MonoBehaviour
{
    [Header("Panel")]
    public GameObject panel;

    [Header("Teleport")]
    public Transform player;
    public Transform teleportPoint;

    [Header("Old Man")]
    public Transform oldMan;
    public Transform oldManTeleportPoint;

    [Header("Canvas")]
    public Transform canvasTransform;

    [Header("Settings")]
    public float delay = 7f;

    Coroutine currentRoutine;

    private void OnEnable()
    {
        StartSequence();
    }

    public void StartSequence()
    {
        if (currentRoutine != null)
        {
            StopCoroutine(currentRoutine);
        }

        currentRoutine = StartCoroutine(TeleportRoutine());
    }

    IEnumerator TeleportRoutine()
    {
        // Повернуть canvas к игроку
        RotateCanvasToPlayer();

        // Показать панель
        if (panel != null)
        {
            panel.SetActive(true);
        }

        // Ждать
        yield return new WaitForSeconds(delay);

        // Телепорт старейшины
        if (oldMan != null && oldManTeleportPoint != null)
        {
            oldMan.SetPositionAndRotation(
                oldManTeleportPoint.position,
                oldManTeleportPoint.rotation
            );
        }

        // Телепорт игрока
        if (player != null && teleportPoint != null)
        {
            CharacterController cc =
                player.GetComponent<CharacterController>();

            if (cc != null)
            {
                cc.enabled = false;
            }

            player.SetPositionAndRotation(
                teleportPoint.position,
                teleportPoint.rotation
            );

            Physics.SyncTransforms();

            if (cc != null)
            {
                cc.enabled = true;
            }
        }

        // Скрыть панель
        if (panel != null)
        {
            panel.SetActive(false);
        }

        currentRoutine = null;
    }

    void RotateCanvasToPlayer()
    {
        if (canvasTransform != null && player != null)
        {
            Vector3 direction =
                canvasTransform.position - player.position;

            direction.y = 0;

            if (direction != Vector3.zero)
            {
                canvasTransform.rotation =
                    Quaternion.LookRotation(direction);
            }
        }
    }

    private void Update()
    {
        RotateCanvasToPlayer();
    }
}