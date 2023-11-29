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
        Vector3 spawnPoint = Vector3.zero;
        Debug.Log(PhotonNetwork.LocalPlayer.ActorNumber);
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
            spawnPoint = spawnPoint1.transform.position;
        else
            spawnPoint = spawnPoint2.transform.position;
        Debug.Log(spawnPoint);
        PhotonNetwork.Instantiate("Player", spawnPoint, Quaternion.identity);
    }
}