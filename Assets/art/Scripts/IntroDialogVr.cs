using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using System.Collections.Generic;

public class IntroDialogueVR : MonoBehaviour
{
    [Header("Dialogue Panels")]
    public GameObject[] panels;

    [Header("Settings")]
    public float panelDuration = 5f;

    [Header("Player Teleport")]
    public Transform player;
    public Transform teleportPoint;

    [Header("Old Man Teleport")]
    public Transform oldMan;
    public Transform oldManTeleportPoint;

    [Header("Canvas")]
    public Transform canvasTransform;

    [Header("Bow Dialogue")]
    public GameObject bowDialogue;

    private int currentPanel = 0;
    private float timer;
    private bool waitingForTeleport = false;

    private UnityEngine.XR.InputDevice rightController;

    private bool lastVRButtonState = false;

    void Start()
    {
        DisableAllPanels();

        if (panels.Length > 0)
        {
            panels[0].SetActive(true);
        }

        timer = panelDuration;

        GetRightController();
    }

    void Update()
    {
        RotateCanvasToPlayer();

        // Проверка VR контроллера
        if (!rightController.isValid)
        {
            GetRightController();
        }

        // Кнопка B на клавиатуре
        bool keyboardButton =
            Keyboard.current != null &&
            Keyboard.current.bKey.wasPressedThisFrame;

        // Кнопка B на VR контроллере
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

        // Последняя панель
        if (waitingForTeleport)
        {
            if (buttonDown)
            {
                TeleportPlayer();
            }

            return;
        }

        timer -= Time.deltaTime;

        // Автоматическое переключение
        if (timer <= 0)
        {
            NextPanel();
        }

        // Следующая панель
        if (buttonDown)
        {
            NextPanel();
        }
    }

    void NextPanel()
    {
        if (currentPanel < panels.Length - 1)
        {
            panels[currentPanel].SetActive(false);

            currentPanel++;

            panels[currentPanel].SetActive(true);

            timer = panelDuration;

            // Последняя панель
            if (currentPanel == panels.Length - 1)
            {
                waitingForTeleport = true;
            }
        }
    }

    void DisableAllPanels()
    {
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }
    }

    void RotateCanvasToPlayer()
    {
        if (canvasTransform != null && player != null)
        {
            Vector3 direction =
                canvasTransform.position - player.position;

            direction.y = 0;

            canvasTransform.rotation =
                Quaternion.LookRotation(direction);
        }
    }

    void TeleportPlayer()
    {
        // PLAYER
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

        // OLD MAN
        if (oldMan != null && oldManTeleportPoint != null)
        {
            oldMan.SetPositionAndRotation(
                oldManTeleportPoint.position,
                oldManTeleportPoint.rotation
            );
        }

        // HIDE INTRO DIALOG
        if (canvasTransform != null)
        {
            canvasTransform.gameObject.SetActive(false);
        }

        // START BOW DIALOG
        if (bowDialogue != null)
        {
            bowDialogue.SetActive(true);
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