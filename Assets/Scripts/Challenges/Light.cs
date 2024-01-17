using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light : MonoBehaviour
{
    private bool isOn = false;
    private Renderer rendererComponent;

    void Start()
    {
        // Get the Renderer component attached to this GameObject
        rendererComponent = GetComponent<Renderer>();
    }

    private void UpdateLight()
    {
        // Toggle the state of the light
        isOn = !isOn;

        // Change the color of the material based on the state
        if (isOn)
            rendererComponent.material.SetColor("_Color", Color.green);
        else
            rendererComponent.material.SetColor("_Color", Color.red);
    }
}
