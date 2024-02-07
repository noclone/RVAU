using Photon.Pun;
using UnityEngine;

public class NetworkPlayerManager : MonoBehaviourPunCallbacks
{
    public GameObject spawnPoint1;
    public GameObject spawnPoint2;
    void Start()
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            PhotonNetwork.Instantiate("Player", spawnPoint1.transform.position, spawnPoint1.transform.rotation);
            GameObject.Find("GroundSectionSpawner").GetComponent<GroundSectionSpawner>().enabled = true;

            PhotonView.Get(this).RPC("InstantiatePlayerVR", RpcTarget.AllBuffered);
        }
        GameObject.Find("GameEngine").GetComponent<GameEngine>().StartGame();
    }

    [PunRPC]
    public void InstantiatePlayerVR()
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
            return;
        PhotonNetwork.Instantiate("PlayerVR", spawnPoint2.transform.position, Quaternion.identity);
    }
}