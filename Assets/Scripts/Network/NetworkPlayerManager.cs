using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class NetworkPlayerManager : MonoBehaviourPunCallbacks
{
    public GameObject spawnPoint1;
    public GameObject spawnPoint2;
    void Start()
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            PhotonNetwork.Instantiate("Player", spawnPoint1.transform.position, Quaternion.identity);
            GameObject.Find("GroundSectionSpawner").GetComponent<GroundSectionSpawner>().enabled = true;
        }
        else
        {
            PhotonNetwork.Instantiate("PlayerVR", spawnPoint2.transform.position, Quaternion.identity);
        }
        GameObject.Find("GameEngine").GetComponent<GameEngine>().StartGame();
    }
}