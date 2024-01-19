using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class Switch: MonoBehaviour
{
    static List<int[]> combinations = new List<int[]>
    {
        new int[] { 0, 2, 4 },
        new int[] { 0, 1, 3 },
        new int[] { 0 }
    };

    public Image On;
    public Image Off;
    private Bulb[] bulbs;
    private int[] combination;

    void Start()
    {
        System.Random random = new System.Random();
        combination = combinations[random.Next(combinations.Count)];
        combinations.Remove(combination);
        bulbs = GameObject.FindGameObjectsWithTag("Bulb").Select(bulb => bulb.GetComponent<Bulb>()).ToArray();
    }

    void Update()
    {
        // If all the bulbs are on, change scene
        if (bulbs.All(bulb => bulb.isOn))
            SceneManager.LoadScene("Game");
    }

    public void ON()
    {
        Off.gameObject.SetActive(true);
        On.gameObject.SetActive(false);

        foreach (int i in combination)
            bulbs[i].Toggle();
    }

    public void OFF()
    {
        Off.gameObject.SetActive(false);
        On.gameObject.SetActive(true);

        foreach (int i in combination)
            bulbs[i].Toggle();
    }
}