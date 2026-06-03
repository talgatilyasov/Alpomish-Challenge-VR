using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HideHandOnGrab : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();

        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        GameObject hand = args.interactorObject.transform.gameObject;

        Renderer[] renderers = hand.GetComponentsInChildren<Renderer>();

        foreach (Renderer r in renderers)
        {
            r.enabled = false;
        }
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        GameObject hand = args.interactorObject.transform.gameObject;

        Renderer[] renderers = hand.GetComponentsInChildren<Renderer>();

        foreach (Renderer r in renderers)
        {
            r.enabled = true;
        }
    }
}