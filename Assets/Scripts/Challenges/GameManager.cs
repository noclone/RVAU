using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    private List<Bulb> bulbs;
    private List<Switch> switches;
    private List<int> switchMapping;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        bulbs = FindObjectsOfType<Bulb>().ToList();
        switches = FindObjectsOfType<Switch>().ToList();
        switchMapping = new List<int> {};

        System.Random random = new System.Random();

        for (int i = 0; i < switches.Count; i++)
        {
            int randomIndex;

            do
            {
                randomIndex = random.Next(0, bulbs.Count);
            } while (switchMapping.Contains(randomIndex));

            switchMapping.Add(randomIndex);

            if (random.Next(0, 2) == 0)
                Toggle(switches[i].GetInstanceID());
        }

        GameObject canvasPC = GameObject.Find("CanvasPC");
        GameObject canvasVR = GameObject.Find("CanvasVR");

        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            canvasPC.SetActive(true);
            canvasVR.SetActive(false);
        } 
        else
        {
            canvasVR.SetActive(true);
            canvasPC.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (bulbs.All(b => b.isOn))
            SceneManager.LoadScene("Game");
    }

    public void Toggle(int switchId)
    {
        int index = switches.FindIndex(s => s.GetInstanceID() == switchId);
        int bulbIndex = switchMapping[index];

        foreach (Bulb bulb in bulbs) {
            if (bulb.transform.position.x == bulbs[bulbIndex].transform.position.x || bulb.transform.position.y == bulbs[bulbIndex].transform.position.y)
                bulb.Toggle();
        }
    }
}
