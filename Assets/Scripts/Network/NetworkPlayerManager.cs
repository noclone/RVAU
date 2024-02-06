using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkPlayerManager : MonoBehaviourPunCallbacks
{
    public GameObject spawnPoint1;
    public GameObject spawnPoint2;
    private Scene currentScene;

    void Start()
    {

        currentScene = SceneManager.GetActiveScene();

        // To be removed
        PhotonNetwork.ConnectUsingSettings();
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            PhotonNetwork.Instantiate("Player", spawnPoint1.transform.position, spawnPoint1.transform.rotation);
            GameObject.Find("GroundSectionSpawner").GetComponent<GroundSectionSpawner>().enabled = true;

            PhotonView.Get(this).RPC("InstantiatePlayerVR", RpcTarget.AllBuffered);
        }
    }

    void Update()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == currentScene.name)
            return;

        if (scene.name != "Game")
        {
            GameObject canvasVR = GameObject.Find("CanvasVR");
            GameObject canvasPC = GameObject.Find("CanvasPC");
            if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
            {
                canvasVR.SetActive(false);
                canvasPC.SetActive(true);
            }
            else
            {
                canvasVR.SetActive(true);
                canvasPC.SetActive(false);
            }
        }

        currentScene = scene;
    }

    // To be removed
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom("Room1", new RoomOptions(), TypedLobby.Default);
    }

    // To be removed
    public override void OnJoinedRoom()
    {
        PhotonNetwork.Instantiate("PlayerVR", spawnPoint2.transform.position, Quaternion.identity);
    }

    [PunRPC]
    public void InstantiatePlayerVR()
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
            return;
        PhotonNetwork.Instantiate("PlayerVR", spawnPoint2.transform.position, Quaternion.identity);
    }
}