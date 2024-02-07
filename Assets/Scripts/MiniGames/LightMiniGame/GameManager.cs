using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject spawnPointPC;
    public GameObject spawnPointVR;
    public GameObject canvasPC;
    public GameObject canvasVR;
    public List<Bulb> bulbs;
    public List<ButtonVR> buttons;

    private List<int> buttonMapping;
    
    public TextMeshProUGUI scoreText;
    private int score;
    private float scoreIncrementInterval = 0.1f;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            GameObject playerPC = PhotonNetwork.Instantiate("Player", spawnPointPC.transform.position, Quaternion.identity);
            Camera camera = playerPC.GetComponent<Camera>();
            Canvas canvas = canvasPC.GetComponent<Canvas>();
            canvas.worldCamera = camera;
            playerPC.GetComponent<NavigationPC>().enabled = false;
            canvasVR.SetActive(false);
            canvasPC.SetActive(true);
            if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("score", out object scoreValue))
            {
                score = (int)scoreValue;
                UpdateScoreText();
            }
        }
        else
        {
            GameObject playerVR = PhotonNetwork.Instantiate("PlayerVR", spawnPointVR.transform.position, Quaternion.identity);
            playerVR.transform.GetChild(1).gameObject.SetActive(false);
            playerVR.GetComponent<Rigidbody>().useGravity = false;
            Camera camera = playerVR.GetComponent<Camera>();
            Canvas canvas = canvasVR.GetComponent<Canvas>();
            playerVR.GetComponent<Navigation>().enabled = false;
            canvas.worldCamera = camera;
            canvasVR.SetActive(true);
            canvasPC.SetActive(false);
        }
        GameObject.Find("GameManager").GetComponent<GameManager>().enabled = true;

        instance = this;
        buttonMapping = new List<int> {};

        System.Random random = new System.Random();

        bool changed = false;

        for (int i = 0; i < buttons.Count; i++)
        {

            int randomIndex;

            do
            {
                randomIndex = random.Next(0, bulbs.Count);
            } while (buttonMapping.Contains(randomIndex));

            buttonMapping.Add(randomIndex);
        }

        for (int i = 0; i < buttons.Count; i++)
        {
            if (random.Next(0, 2) == 0)
            {
                RPC_Toggle(buttons[i].id);
                changed = true;
            }
        }

        if (!changed)
        {
            RPC_Toggle(buttons[random.Next(0, buttons.Count)].id);
        }
    }
    
    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + "<mspace=0.6em>" + score.ToString("D6");
    }

    [PunRPC]
    void SetVictory()
    {
        PhotonNetwork.LoadLevel("Game");
    }


    public void Toggle(int buttonId)
    {
        PhotonView.Get(this).RPC("RPC_Toggle", RpcTarget.AllBuffered, buttonId);
    }


    [PunRPC]
    public void RPC_Toggle(int buttonId)
    {
        int bulbIndex = buttonMapping[buttonId];
        foreach (Bulb bulb in bulbs) {
            if (bulb.transform.position.x == bulbs[bulbIndex].transform.position.x || bulb.transform.position.y == bulbs[bulbIndex].transform.position.y)
                bulb.Toggle();
        }
        if (bulbs.All(b => b.isOn))
            PhotonView.Get(this).RPC("SetVictory", RpcTarget.AllBuffered);
    }

    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            timer += Time.deltaTime;
            if (timer >= scoreIncrementInterval)
            {
                score--;
                UpdateScoreText();
                timer = 0.0f;

                ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable();
                customProperties.Add("score", score);
                PhotonNetwork.CurrentRoom.SetCustomProperties(customProperties);
            }
        }
    }
}