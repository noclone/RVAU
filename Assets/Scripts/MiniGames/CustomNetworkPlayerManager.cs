using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class CustomNetworkPlayerManager : MonoBehaviourPunCallbacks
{
    public GameObject spawnPointPC;
    public GameObject spawnPointVR;
    void Start()
    {
        // To be removed
        PhotonNetwork.ConnectUsingSettings();
        /*if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            PhotonNetwork.Instantiate("Player", spawnPoint1.transform.position, Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate("PlayerVR", spawnPoint2.transform.position, Quaternion.identity);
        }*/
    }

    // To be removed
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions(), TypedLobby.Default);
    }

    // To be removed
    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            PhotonNetwork.Instantiate("Player", spawnPointPC.transform.position, Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate("PlayerVR", spawnPointVR.transform.position, Quaternion.identity);
        }
    }
}