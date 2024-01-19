using UnityEngine;

public class GroundTile : MonoBehaviour
{
    public GameObject JumpingObstacle;
    public GameObject WallObstacle;

    void SpawnObstacle()
    {
        GameObject[] obstacles = { JumpingObstacle, WallObstacle };
        // Select a random obstacle
        GameObject obstaclePrefab = obstacles[Random.Range(0, obstacles.Length)];
        if (obstaclePrefab == JumpingObstacle)
        {
            for (int i = 1; i < 4; i++)
            {
                Transform spawnPoint = transform.GetChild(i).transform;
                Instantiate(obstaclePrefab, spawnPoint.position, Quaternion.identity, transform);
            }
        }
        else
        {
            int randomPoint = Random.Range(1, 4);
            Transform spawnPoint = transform.GetChild(randomPoint).transform;
            Instantiate(obstaclePrefab, spawnPoint.position, Quaternion.identity, transform);
        }
    }

    void Start()
    {
        SpawnObstacle();
    }
}