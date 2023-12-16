using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class NetworkPlayerManager : MonoBehaviourPun
{
    public GameObject spawnPoint1;
    public GameObject spawnPoint2;
    void Start()
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
        {
            PhotonNetwork.Instantiate("Player", spawnPoint1.transform.position, Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate("PlayerVR", spawnPoint2.transform.position, Quaternion.identity);
        }
    }
}