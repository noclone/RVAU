using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class NetworkPlayer : MonoBehaviour
{
    void Start()
    {
        if (gameObject.GetComponent<PhotonView>().IsMine)
        {
            gameObject.GetComponent<Navigation>().enabled = false;
        }
    }
}