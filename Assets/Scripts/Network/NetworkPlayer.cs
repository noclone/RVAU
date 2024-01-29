using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public class NetworkPlayer : MonoBehaviour
{
    void Start()
    {
        if (!gameObject.GetComponent<PhotonView>().IsMine)
        {
            if (gameObject.GetComponent<Camera>() != null)
            {
                gameObject.GetComponent<Camera>().enabled = false;
                gameObject.GetComponent<AudioListener>().enabled = false;
            }
            else
            {
                gameObject.GetComponent<Navigation>().enabled = false;
                gameObject.transform.GetChild(0).GetChild(0).GetComponent<Camera>().enabled = false;
                gameObject.transform.GetChild(0).GetChild(0).GetComponent<AudioListener>().enabled = false;
            }
        }
    }
}