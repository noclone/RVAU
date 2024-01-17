using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GroundSectionSpawner : MonoBehaviourPunCallbacks
{
    private Vector3 nextSpawnPoint;
    void Start()
    {
    }

    public void spawnSection()
    {
        GameObject section = PhotonNetwork.Instantiate("GroundSection", nextSpawnPoint, Quaternion.identity);
        nextSpawnPoint = section.transform.GetChild(0).transform.position;
    }

    // To be removed and move content to start
    public override void OnJoinedRoom()
    {
        for (int i = 0; i < 2; i++)
            spawnSection();
    }

    void Update()
    {

    }
}