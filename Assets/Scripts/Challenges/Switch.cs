using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Assuming Light is a custom class you've created for your lights
using Light = Assets.Scripts.Challenges.Light;

public class Switch : MonoBehaviour
{
    public bool switchState = false; // False = Off, True = On
    private Light[] connections; // Array of lights controlled by this switch

    void Start()
    {
        Light[] lights = FindObjectsOfType<Light>();

        // Use the instance ID as a unique seed for each Switch
        Random.InitState(GetInstanceID());

        // Initialize the switches randomly with the specified combinations
        int randomCombination = Random.Range(1, 4); // Random number between 1 and 3

        switch (randomCombination)
        {
            case 1:
                // Combination: Switches 1, 3, and 5 are on
                connections = new Light[] { lights[0], lights[2], lights[4] };
                break;

            case 2:
                // Combination: Switches 2 and 4 are on
                connections = new Light[] { lights[1], lights[3] };
                break;

            case 3:
                // Combination: Switch 1 is on
                connections = new Light[] { lights[0] };
                break;

            default:
                Debug.LogError("Invalid combination number");
                break;
        }
    }

    // Call if player clicks on the Switch
    void OnMouseDown()
    {
        switchState = !switchState; // Toggle the switch state

        // Loop through all lights in the connections array
        for (int i = 0; i < connections.Length; ++i)
        {
            // Toggle the light
            connections[i].ToggleLight();
        }

        // Rotate the switch 180 degrees along the z-axis
        transform.Rotate(0, 0, 180);
    }
}
