using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;


public class ButtonVR : MonoBehaviour
{
    public GameObject button;
    bool isPressed;
    public int id;

    void Start()
    {
        isPressed = false;
    }

    public void ButtonPress()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().Toggle(id);
        if (!isPressed)
        {
            button.transform.Translate(Vector3.down * 0.1f);
            button.GetComponent<MeshRenderer>().material.color = Color.green;
            isPressed = true;
        }
        else
        {
            button.transform.Translate(Vector3.up * 0.1f);
            button.GetComponent<MeshRenderer>().material.color = Color.red;
            isPressed = false;
        }
    }
}