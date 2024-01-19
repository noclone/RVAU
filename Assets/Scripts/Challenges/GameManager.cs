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
        switchMapping = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };

        System.Random random = new System.Random();
        int n = switchMapping.Count;

        for (int i = n - 1; i > 0; i--)
        {
            int j = random.Next(0, i + 1);

            int tmp = switchMapping[i];
            switchMapping[i] = switchMapping[j];
            switchMapping[j] = tmp;
        }


        // GameObject canvasPC = GameObject.Find("CanvasPC");
        // GameObject canvasVR = GameObject.Find("CanvasVR");

        // if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        // {
        //     canvasPC.SetActive(true);
        //     canvasVR.SetActive(false);
        // } 
        // else
        // {
        //     canvasVR.SetActive(true);
        //     canvasPC.SetActive(false);
        // }
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
        Debug.Log(index);
        int bulbIndex = switchMapping[index];

        foreach (Bulb bulb in bulbs) {
            if (bulb.transform.position.x == bulbs[bulbIndex].transform.position.x || bulb.transform.position.y == bulbs[bulbIndex].transform.position.y)
                bulb.Toggle();
        }
    }
}
