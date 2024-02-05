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

            if (!PhotonNetwork.IsMasterClient)
            {
                HideObstacles(true);
                int rnd = Random.Range(0, 3);
                rnd = 2;
                if (rnd == 0)
                    GameObject.Find("GameEngine").GetComponent<GameEngine>().LoadTextMiniGame();
                else if (rnd == 1)
                    GameObject.Find("GameEngine").GetComponent<GameEngine>().LoadColorMiniGame();
                else if (rnd == 2)
                {
                    HideObstacles(false);
                    PhotonView.Get(this).RPC("GenerateNextSection", RpcTarget.AllBuffered);
                }
            }
        }
    }

    [PunRPC]
    public void GenerateNextSection()
    {
        Debug.Log("Generating next section out");
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Generating next section in if");
            GameObject.Find("GroundSectionSpawner").GetComponent<GroundSectionSpawner>().spawnSection();
        }
    }

    public void HideObstacles(bool state)
    {
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (GameObject obstacle in obstacles)
        {
            obstacle.GetComponent<MeshRenderer>().enabled = state;
        }
    }
}