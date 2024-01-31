using Photon.Pun;
using UnityEngine;

public class GroundTile : MonoBehaviour
{
    void SpawnObstacle()
    {
        int patternType = Random.Range(0, 4);
        int randomObstacle;
        string obstacleName;
        switch (patternType)
        {
            case 0:
                // Jumping obstacle is spawned in all lanes
                for (int i = 1; i < 4; i++)
                {
                    Transform spawnPoint = transform.GetChild(i).transform;
                    randomObstacle = Random.Range(0, 2);
                    obstacleName = randomObstacle == 0 ? "Concrete_Barrier_1" : "Metal_Barrier_1";
                    if (randomObstacle == 1)
                        spawnPoint.position = new Vector3(spawnPoint.position.x - 0.8f, spawnPoint.position.y, spawnPoint.position.z);
                    PhotonNetwork.Instantiate(obstacleName, spawnPoint.position, Quaternion.Euler(0, 90, 0));
                }
                break;
            case 1:
                // Wall obstacle is spawned in a random lane
                int randomWallPoint = Random.Range(1, 4);
                Transform wallSpawnPoint = transform.GetChild(randomWallPoint).transform;
                PhotonNetwork.Instantiate("Concrete_Barrier_2", wallSpawnPoint.position, Quaternion.Euler(0, 90, 0));
                break;
            case 2:
                // Two walls followed by a jumping obstacle
                for (int i = 1; i <= 2; i++)
                {
                    Transform wallSpawnPoint2 = transform.GetChild(i).transform;
                    PhotonNetwork.Instantiate("Concrete_Barrier_2", wallSpawnPoint2.position, Quaternion.Euler(0, 90, 0));
                }
                Transform jumpSpawnPoint = transform.GetChild(3).transform;
                randomObstacle = Random.Range(0, 2);
                obstacleName = randomObstacle == 0 ? "Concrete_Barrier_1" : "Metal_Barrier_1";
                if (randomObstacle == 1)
                    jumpSpawnPoint.position = new Vector3(jumpSpawnPoint.position.x - 0.8f, jumpSpawnPoint.position.y, jumpSpawnPoint.position.z);
                PhotonNetwork.Instantiate(obstacleName, jumpSpawnPoint.position, Quaternion.Euler(0, 90, 0));
                break;
            case 3:
                // Two jumping obstacles followed by a wall
                for (int i = 1; i <= 2; i++)
                {
                    Transform jumpSpawnPoint2 = transform.GetChild(i).transform;
                    randomObstacle = Random.Range(0, 2);
                    obstacleName = randomObstacle == 0 ? "Concrete_Barrier_1" : "Metal_Barrier_1";
                    if (randomObstacle == 1)
                        jumpSpawnPoint2.position = new Vector3(jumpSpawnPoint2.position.x - 0.8f, jumpSpawnPoint2.position.y, jumpSpawnPoint2.position.z);
                    PhotonNetwork.Instantiate(obstacleName, jumpSpawnPoint2.position, Quaternion.Euler(0, 90, 0));
                }
                Transform wallSpawnPoint3 = transform.GetChild(3).transform;
                PhotonNetwork.Instantiate("Concrete_Barrier_2", wallSpawnPoint3.position, Quaternion.Euler(0, 90, 0));
                break;

            // TODO: Add more patterns
        }
    }

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
            SpawnObstacle();
    }
}