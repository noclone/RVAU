using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTile : MonoBehaviour
{
    public GameObject obstaclePrefab;
    private GroundSectionSpawner groundSectionSpawner;

    void SpawnObstacle()
    {
        // Random index between spawn points
        int obstacleSpawnIndex = Random.Range(1, 4);
        Transform spawnPoint = transform.GetChild(obstacleSpawnIndex).transform;
        Instantiate(obstaclePrefab, spawnPoint.position, Quaternion.identity, transform);
    }

    private void OnTriggerExit(Collider other)
    {
        groundSectionSpawner.spawnSection();
        Destroy(gameObject, 2);
    }

    void Start()
    {
        SpawnObstacle();
        groundSectionSpawner = FindObjectOfType<GroundSectionSpawner>();
    }
}