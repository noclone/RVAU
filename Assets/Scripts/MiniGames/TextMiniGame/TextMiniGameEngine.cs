using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

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
    public TextMeshProUGUI initialWordText;
    public TMP_InputField inputField;
    public Button submitButton;
    public GameObject errorText;

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
        submitButton.onClick.AddListener(CheckVictoryConditions);
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
        isLoaded = true; 
        initialWordText.text = initialWord;
    }

    void Update()
    {
        if (!isLoaded)
            return;
    }
    
    void CheckVictoryConditions()
    {
        if (inputField.text == answerWord)
        {
            // Set victory to true for all players
            PhotonView.Get(this).RPC("SetVictory", RpcTarget.AllBuffered);
        }
        else
        {
            errorText.SetActive(true);
        }
    }

    [PunRPC]
    void SetVictory()
    {
        PhotonNetwork.LoadLevel("Game");
    }
}