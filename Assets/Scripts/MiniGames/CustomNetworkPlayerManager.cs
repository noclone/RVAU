using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomNetworkPlayerManager : MonoBehaviourPunCallbacks
{
    public GameObject spawnPointPC;
    public GameObject spawnPointVR;
    public GameObject canvasVR;
    public GameObject canvasPC;
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
        PhotonNetwork.JoinOrCreateRoom("RoomMiniGame", new RoomOptions(), TypedLobby.Default);
    }

    // To be removed
    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            GameObject playerPC = PhotonNetwork.Instantiate("Player", spawnPointPC.transform.position, Quaternion.identity);
            Camera camera = playerPC.GetComponent<Camera>();
            Canvas canvas = canvasPC.GetComponent<Canvas>();
            canvas.worldCamera = camera;
            canvasVR.SetActive(false);
        }
        else
        {
            GameObject playerVR = PhotonNetwork.Instantiate("PlayerVR", spawnPointVR.transform.position, Quaternion.identity);
            Camera camera = playerVR.GetComponent<Camera>();
            Canvas canvas = canvasVR.GetComponent<Canvas>();
            canvas.worldCamera = camera;
        }
        GameObject.Find("ScriptHolder").GetComponent<ColorMiniGameScript>().enabled = true;
    }
}