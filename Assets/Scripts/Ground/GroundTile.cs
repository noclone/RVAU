using UnityEngine;

public class GroundTile : MonoBehaviour
{
    public GameObject JumpingObstacle;
    public GameObject WallObstacle;

    void SpawnObstacle()
    {
        int patternType = Random.Range(0, 4);

        switch (patternType)
        {
            case 0:
                // Jumping obstacle is spawned in all lanes
                for (int i = 1; i < 4; i++)
                {
                    Transform spawnPoint = transform.GetChild(i).transform;
                    Instantiate(JumpingObstacle, spawnPoint.position, Quaternion.identity, transform);
                }
                break;
            case 1:
                // Wall obstacle is spawned in a random lane
                int randomWallPoint = Random.Range(1, 4);
                Transform wallSpawnPoint = transform.GetChild(randomWallPoint).transform;
                Instantiate(WallObstacle, wallSpawnPoint.position, Quaternion.identity, transform);
                break;
            case 2:
                // Two walls followed by a jumping obstacle
                for (int i = 1; i <= 2; i++)
                {
                    Transform wallSpawnPoint2 = transform.GetChild(i).transform;
                    Instantiate(WallObstacle, wallSpawnPoint2.position, Quaternion.identity, transform);
                }
                Transform jumpSpawnPoint = transform.GetChild(3).transform;
                Instantiate(JumpingObstacle, jumpSpawnPoint.position, Quaternion.identity, transform);
                break;
            case 3:
                // Two jumping obstacles followed by a wall
                for (int i = 1; i <= 2; i++)
                {
                    Transform jumpSpawnPoint2 = transform.GetChild(i).transform;
                    Instantiate(JumpingObstacle, jumpSpawnPoint2.position, Quaternion.identity, transform);
                }
                Transform wallSpawnPoint3 = transform.GetChild(3).transform;
                Instantiate(WallObstacle, wallSpawnPoint3.position, Quaternion.identity, transform);
                break;
            
            // TODO: Add more patterns
        }
    }

    void Start()
    {
        SpawnObstacle();
    }
}
