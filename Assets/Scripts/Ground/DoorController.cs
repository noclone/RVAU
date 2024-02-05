using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private Animator door;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            door.Play("DoorOpen", 0, 0.0f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        int rnd = Random.Range(0, 2);
        if (rnd == 0)
            GameObject.Find("GameEngine").GetComponent<GameEngine>().LoadTextMiniGame();
        else if (rnd == 1)
            GameObject.Find("GameEngine").GetComponent<GameEngine>().LoadColorMiniGame();
    }
}