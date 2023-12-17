using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class NetworkPlayer : MonoBehaviour
{
    void Start()
    {
        if (!gameObject.GetComponent<PhotonView>().IsMine)
        {
            gameObject.GetComponent<Navigation>().enabled = false;
            if (gameObject.GetComponentInChildren<Camera>() != null)
                gameObject.GetComponentInChildren<Camera>().enabled = false;
            else
                gameObject.transform.GetChild(0).GetComponentInChildren<Camera>().enabled = false;
        }
    }
}