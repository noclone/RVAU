using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private Animator door;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            door.Play("DoorOpen", 0, 0.0f);
            HideObstacles(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        int rnd = Random.Range(0, 3);
        rnd = 2;
        if (rnd == 0)
            GameObject.Find("GameEngine").GetComponent<GameEngine>().LoadTextMiniGame();
        else if (rnd == 1)
            GameObject.Find("GameEngine").GetComponent<GameEngine>().LoadColorMiniGame();
        else if (rnd == 2)
            HideObstacles(true);
    }

    private void HideObstacles(bool state)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (GameObject obstacle in obstacles)
        {
            obstacle.GetComponent<MeshRenderer>().enabled = state;
        }
    }
}