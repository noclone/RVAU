using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;

public class TextMiniGameEngine : MonoBehaviour
{

    private bool victory = false;
    private bool isLoaded;
    
    public String answerWord;
    public String initialWord;

    public GameObject spawnPointPC;
    public GameObject spawnPointVR;
    public GameObject canvasVR;
    public GameObject canvasPC;

    private String[] possibleWords = new[]
    {
        "about",
        "after",
        "again",
        "below",
        "could",
        "every",
        "first",
        "found",
        "great",
        "house",
        "large",
        "learn",
        "never",
        "other",
        "place",
        "plant",
        "point",
        "right",
        "small",
        "sound",
        "spell",
        "still",
        "study",
        "their",
        "there",
        "these",
        "thing",
        "think",
        "three",
        "water",
        "where",
        "which",
        "world",
        "would",
        "write"
    };

    void Start()
    {
        Debug.Log("Start TextMiniGameEngine");
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            GameObject playerPC = PhotonNetwork.Instantiate("Player", spawnPointPC.transform.position, Quaternion.identity);
            Camera camera = playerPC.GetComponent<Camera>();
            Canvas canvas = canvasPC.GetComponent<Canvas>();
            canvas.worldCamera = camera;
            canvasPC.SetActive(true);
        }
        else
        {
            GameObject playerVR = PhotonNetwork.Instantiate("PlayerVR", spawnPointVR.transform.position, Quaternion.identity);
            playerVR.transform.GetChild(1).gameObject.SetActive(false);
            playerVR.GetComponent<Rigidbody>().useGravity = false;
            playerVR.GetComponent<Navigation>().enabled = false;
            Camera camera = playerVR.GetComponent<Camera>();
            Canvas canvas = canvasVR.GetComponent<Canvas>();
            canvas.worldCamera = camera;
            canvasVR.SetActive(true);
        }

        if (PhotonNetwork.IsMasterClient)
        {
            InitializeSolutionWord();
        }
    }

    private void InitializeSolutionWord()
    {
        answerWord = possibleWords[UnityEngine.Random.Range(0, possibleWords.Length)];
        Debug.Log("The answer is: " + answerWord);
        
        // Send RPC to initialize colors on all clients
        PhotonView.Get(this).RPC("InitSolutionWord", RpcTarget.AllBuffered, answerWord);
    }
    
    [PunRPC]
    void InitSolutionWord(String answer)
    {
        answerWord = answer;
        char[] answerChars = answerWord.ToCharArray();
        for (int i = 0; i < 3; i++)
        {
            int index = UnityEngine.Random.Range(0, answerChars.Length);
            answerChars[index] = '_';
        }
        initialWord = new String(answerChars);
        Debug.Log("The initial word is: " + initialWord);
        isLoaded = true;
    }

    void Update()
    {
        if (!isLoaded)
            return;

        CheckVictoryConditions();
    }

    void CheckVictoryConditions()
    {
        bool gameVictory = false;

        if (gameVictory)
        {
            // Set victory to true for all players
            PhotonView.Get(this).RPC("SetVictory", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    void SetVictory()
    {
        PhotonNetwork.LoadLevel("Game");
    }
}