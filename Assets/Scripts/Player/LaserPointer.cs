using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class LaserPointer : MonoBehaviour
{
    private XRController xrController;
    private LineRenderer lineRenderer;

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

        if (xrController.inputDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerPressed) && triggerPressed)
        {
            HandleTriggerPress();
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
            Debug.Log("Hit " + hit.collider.gameObject.name);
            if (hit.collider.gameObject.name == "Cover")
            {
                Debug.Log("HERE");
                Vector3 position = hit.collider.gameObject.transform.position;
                hit.collider.gameObject.transform.position = new Vector3(position.x,
                    hit.transform.position.y, position.z);
            }
        }
    }
}