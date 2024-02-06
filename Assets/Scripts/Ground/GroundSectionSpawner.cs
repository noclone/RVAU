using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GroundSectionSpawner : MonoBehaviourPunCallbacks
{
    private Vector3 nextSpawnPoint;
    void Start()
    {
        nextSpawnPoint = new Vector3(0, 0, 25);

        if (PhotonNetwork.IsMasterClient)
            ResetAll();
    }

    public void spawnSection()
    {
        GameObject section = PhotonNetwork.Instantiate("GroundSection", nextSpawnPoint, Quaternion.identity);
        nextSpawnPoint = section.transform.GetChild(0).transform.position;
    }

    private void SpawnStartSections()
    {
        PhotonNetwork.Instantiate("BaseTiles", Vector3.zero, Quaternion.identity);
        for (int i = 0; i < 2; i++)
            spawnSection();
    }

    public void ResetAll()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>() ;
        foreach(GameObject go in allObjects)
        {
            if (go.activeInHierarchy && go.name.Contains("(Clone)") && (go.CompareTag("Ground") || go.CompareTag("Obstacle")))
                PhotonNetwork.Destroy(go);
        }

        nextSpawnPoint = new Vector3(0, 0, 25);
        SpawnStartSections();
    }
}