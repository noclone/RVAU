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
        GameObject.Find("GameEngine").GetComponent<GameEngine>().LoadTextMiniGame();
    }
}