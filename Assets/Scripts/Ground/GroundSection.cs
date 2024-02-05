using System;
using UnityEngine;

public class GroundSection : MonoBehaviour
{
    private GroundSectionSpawner groundSectionSpawner;
    void Start()
    {
        groundSectionSpawner = FindObjectOfType<GroundSectionSpawner>();
    }

    private void OnTriggerExit(Collider other)
    {
        int rnd = UnityEngine.Random.Range(0, 2);
        if (rnd == 0)
            GameObject.Find("GameEngine").GetComponent<GameEngine>().LoadTextMiniGame();
        else if (rnd == 1)
            GameObject.Find("GameEngine").GetComponent<GameEngine>().LoadColorMiniGame();
    }
}