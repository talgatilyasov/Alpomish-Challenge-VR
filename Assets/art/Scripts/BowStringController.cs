using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class BowStringController : MonoBehaviour
{
    [SerializeField]
    private BowString bowStringRenderer;

    private XRGrabInteractable interactable;

    [SerializeField]
    private Transform midPointGrabObject, midPointVisualObject, midPointParent;

    [SerializeField]
    private float bowStringStretchLimit = 0.3f;

    private Transform interactor;

    // Сила натяжения
    private float strength;

    // События
    public UnityEvent OnBowPulled;
    public UnityEvent<float> OnBowReleased;

    private void Awake()
    {
        interactable = midPointGrabObject.GetComponent<XRGrabInteractable>();
    }

    private void Start()
    {
        interactable.selectEntered.AddListener(PrepareBowString);
        interactable.selectExited.AddListener(ResetBowString);
    }

    private void ResetBowString(SelectExitEventArgs arg0)
    {
        // Вызываем событие отпускания тетивы
        OnBowReleased?.Invoke(strength);

        strength = 0;

        interactor = null;

        midPointGrabObject.localPosition = Vector3.zero;
        midPointVisualObject.localPosition = Vector3.zero;

        bowStringRenderer.CreateString(null);
    }

    private void PrepareBowString(SelectEnterEventArgs arg0)
    {
        interactor = arg0.interactorObject.transform;

        // Событие начала натяжения
        OnBowPulled?.Invoke();
    }

    private void Update()
    {
        if (interactor != null)
        {
            // Переводим позицию в локальные координаты
            Vector3 midPointLocalSpace =
                midPointParent.InverseTransformPoint(midPointGrabObject.position);

            // Работаем по Y
            float midPointLocalYAbs = Mathf.Abs(midPointLocalSpace.y);

            HandleStringPushedBackToStart(midPointLocalSpace);

            HandleStringPulledBackToLimit(midPointLocalYAbs, midPointLocalSpace);

            HandlePullingString(midPointLocalYAbs, midPointLocalSpace);

            bowStringRenderer.CreateString(midPointVisualObject.position);
        }
    }

    private void HandlePullingString(float midPointLocalYAbs, Vector3 midPointLocalSpace)
    {
        // Между стартом и лимитом натяжения
        if (midPointLocalSpace.y < 0 && midPointLocalYAbs < bowStringStretchLimit)
        {
            // Рассчитываем силу натяжения от 0 до 1
            strength = Remap(midPointLocalYAbs, 0, bowStringStretchLimit, 0, 1);

            // Движение только по Y
            midPointVisualObject.localPosition =
                new Vector3(0, midPointLocalSpace.y, 0);
        }
    }

    private void HandleStringPulledBackToLimit(float midPointLocalYAbs, Vector3 midPointLocalSpace)
    {
        // Ограничение максимального натяжения
        if (midPointLocalSpace.y < 0 && midPointLocalYAbs >= bowStringStretchLimit)
        {
            strength = 1;

            midPointVisualObject.localPosition =
                new Vector3(0, -bowStringStretchLimit, 0);
        }
    }

    private void HandleStringPushedBackToStart(Vector3 midPointLocalSpace)
    {
        if (midPointLocalSpace.y >= 0)
        {
            strength = 0;

            midPointVisualObject.localPosition = Vector3.zero;
        }
    }

    // Перевод значения в диапазон 0-1
    private float Remap(float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        return (value - fromMin) / (fromMax - fromMin) * (toMax - toMin) + toMin;
    }
}