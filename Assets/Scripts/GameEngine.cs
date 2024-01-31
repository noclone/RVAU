using Photon.Pun;
using TMPro;
using UnityEngine;

public class GameEngine : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverScoreText;

    public float scoreIncrementInterval = 1.0f;
    private float timer;
    private ulong score;
    private bool isGameOver;
    private bool hasGameStarted;

    void Start()
    {
        score = 0;
        UpdateScoreText();
    }

    void Update()
    {
        if (!isGameOver && hasGameStarted)
            UpdateTimer();
    }

    void UpdateTimer()
    {
        timer += Time.deltaTime;

        if (timer >= scoreIncrementInterval)
        {
            score++;
            UpdateScoreText();
            timer = 0.0f;
        }
    }

    void UpdateScoreText()
    {
        scoreText.text = "Score: " + "<mspace=0.6em>" + score.ToString("D6");
    }

    public void EndGame()
    {
        isGameOver = true;
        gameOverPanel.SetActive(true);
        gameOverScoreText.text = "Score: " + score;
    }

    public void StartGame()
    {
        hasGameStarted = true;
    }

    public void LoadColorMiniGame()
    {
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("RPC_LoadColorMiniGame", RpcTarget.All);
    }

    [PunRPC]
    private void RPC_LoadColorMiniGame()
    {
        PhotonNetwork.LoadLevel("MiniGame");
    }
}