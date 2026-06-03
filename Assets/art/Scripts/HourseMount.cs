using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using System.Collections.Generic;

public class HorseMount : MonoBehaviour
{
    [Header("References")]
    public Transform player;

    public Transform saddlePoint;

    [Header("UI")]
    public GameObject mountUI;

    [Header("Horse Control")]
    public HorseController horseController;

    [Header("Mount Distance")]
    public float mountDistance = 3f;

    private bool mounted = false;

    private UnityEngine.XR.InputDevice rightController;

    private bool lastVRButtonState = false;

    void Start()
    {
        if (mountUI != null)
        {
            mountUI.SetActive(false);
        }

        if (horseController != null)
        {
            horseController.enabled = false;
        }

        GetRightController();
    }

    void Update()
    {
        // Проверка VR контроллера
        if (!rightController.isValid)
        {
            GetRightController();
        }

        // Дистанция до лошади
        float distance =
            Vector3.Distance(
                player.position,
                saddlePoint.position
            );

        bool playerNear =
            distance <= mountDistance;

        // UI
        if (mountUI != null)
        {
            mountUI.SetActive(
                playerNear && !mounted
            );
        }

        if (!playerNear || mounted)
            return;

        // Клавиатура B
        bool keyboardButton =
            Keyboard.current != null &&
            Keyboard.current.bKey.wasPressedThisFrame;

        // VR кнопка B
        bool vrButton = false;

        rightController.TryGetFeatureValue(
            UnityEngine.XR.CommonUsages.secondaryButton,
            out vrButton
        );

        bool vrButtonDown =
            vrButton && !lastVRButtonState;

        lastVRButtonState = vrButton;

        // Общая кнопка
        bool buttonDown =
            keyboardButton || vrButtonDown;

        if (buttonDown)
        {
            MountHorse();
        }
    }

    void MountHorse()
    {
        mounted = true;

        if (mountUI != null)
        {
            mountUI.SetActive(false);
        }

        CharacterController cc =
            player.GetComponent<CharacterController>();

        Rigidbody rb =
            player.GetComponent<Rigidbody>();

        // Выключаем CharacterController
        if (cc != null)
        {
            cc.enabled = false;
        }

        // Выключаем gravity
        if (rb != null)
        {
            rb.useGravity = false;

            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            rb.isKinematic = true;
        }

        // Делаем игрока child лошади
        player.SetParent(saddlePoint);

        // Сажаем на седло
        player.localPosition = Vector3.zero;
        player.localRotation = Quaternion.identity;

        Physics.SyncTransforms();

        // Включаем управление лошадью
        if (horseController != null)
        {
            horseController.enabled = true;
        }
    }

    void GetRightController()
    {
        List<UnityEngine.XR.InputDevice> devices =
            new List<UnityEngine.XR.InputDevice>();

        InputDevices.GetDevicesAtXRNode(
            XRNode.RightHand,
            devices
        );

        if (devices.Count > 0)
        {
            rightController = devices[0];
        }
    }
}