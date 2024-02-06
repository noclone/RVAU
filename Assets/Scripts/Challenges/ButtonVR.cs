using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;


public class ButtonVR : MonoBehaviour
{
    public GameObject button;
    public UnityEvent onPress;
    public UnityEvent onRelease;
    
    GameObject presser;
    bool isPressed = false;


    // Start is called before the first frame update
    void Start()
    {
        isPressed = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isPressed)
        {
            button.transform.Translate(Vector3.down * 0.1f);
            presser = other.gameObject;
            onPress.Invoke();
            isPressed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (presser == other.gameObject)
        {
            button.transform.Translate(Vector3.up * 0.1f);
            onRelease.Invoke();
            isPressed = false;
        }
    }

    public void ButtonPress()
    {
        PhotonView.Get(this).RPC("GameManager.instance.Toggle", RpcTarget.AllBuffered, this.GetInstanceID());
    }
}
