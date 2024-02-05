using UnityEngine;
using Photon.Pun;

public class DoorController : MonoBehaviour
{
    [SerializeField] private Animator door;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            door.Play("DoorOpen", 0, 0.0f);
            HideObstacles(true);

            int rnd = Random.Range(0, 3);

            if (rnd == 0)
                GameObject.Find("GameEngine").GetComponent<GameEngine>().LoadTextMiniGame();
            else if (rnd == 1)
                GameObject.Find("GameEngine").GetComponent<GameEngine>().LoadColorMiniGame();
            else if (rnd == 2)
                HideObstacles(false);
        }
    }

    private void HideObstacles(bool state)
    {
        if (PhotonNetwork.IsMasterClient)
            return;
        
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (GameObject obstacle in obstacles)
        {
            obstacle.GetComponent<MeshRenderer>().enabled = state;
        }

        GameObject.Find("GroundSectionSpawner").GetComponent<GroundSectionSpawner>().spawnSection();
    }
}