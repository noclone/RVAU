using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit.UI;

public class TextMiniGameEngine : MonoBehaviour
{

    private bool victory = false;
    private bool isLoaded;
    private bool isDeactivated;

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
    public TextMeshProUGUI scoreText;
    private int score;
    private float scoreIncrementInterval = 0.1f;
    private float timer;

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
            playerPC.GetComponent<NavigationPC>().enabled = false;
            canvas.worldCamera = camera;
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
            playerVR.GetComponent<Navigation>().enabled = false;
            playerVR.transform.GetChild(0).GetChild(1).GetComponent<LaserPointer>().enabled = true;
            playerVR.transform.GetChild(0).GetChild(2).GetComponent<LaserPointer>().enabled = true;
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

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + "<mspace=0.6em>" + score.ToString("D6");
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
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1 && !isDeactivated)
        {
            GameObject eventSystem = GameObject.Find("EventSystem");
            if (eventSystem.GetComponent<XRUIInputModule>() == null)
                return;
            isDeactivated = true;
            eventSystem.GetComponent<XRUIInputModule>().enabled = false;
        }

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

    void CheckVictoryConditions()
    {
        if (inputField.text == answerWord)
        {
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