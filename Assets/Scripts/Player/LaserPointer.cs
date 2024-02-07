using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class LaserPointer : MonoBehaviour
{
    private XRController xrController;
    private LineRenderer lineRenderer;
    private bool triggerPressedLastFrame = false;

    private void Start()
    {
        xrController = GetComponent<XRController>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (xrController.inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 position))
        {
            UpdateLaser(position);
        }

        if (xrController.inputDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerPressed))
        {
            if (triggerPressed)
            {
                HandleTriggerPress();
            }

            triggerPressedLastFrame = triggerPressed;
        }
    }

    private void UpdateLaser(Vector2 position)
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position + transform.forward * 10f);
    }

    private void HandleTriggerPress()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if (hit.collider.CompareTag("Switch") && !triggerPressedLastFrame)
            {
                hit.collider.GetComponent<ButtonVR>().ButtonPress();
            }
            if (hit.collider.gameObject.name == "Cover")
            {
                hit.collider.gameObject.transform.position = new Vector3(hit.collider.gameObject.transform.position.x, hit.point.y, hit.collider.gameObject.transform.position.z);
            }
        }
    }
}