using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bulb : MonoBehaviour
{
    public bool isOn = true;

    public void Start()
    {
        this.GetComponent<Image>().color = Color.yellow;
    }


    public void Toggle()
    {
        isOn = !isOn;
        
        if (isOn)
            this.GetComponent<Image>().color = Color.yellow;
        else
            this.GetComponent<Image>().color = Color.gray;
    }
}
