using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bulb : MonoBehaviour
{
    public bool isOn = false;

    public void Toggle()
    {
        isOn = !isOn;
        
        if (isOn)
            this.GetComponent<Image>().color = Color.green;
        else
            this.GetComponent<Image>().color = Color.red;
    }
}
