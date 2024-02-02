using Photon.Pun;
using TMPro;
using UnityEngine;

public class GameEngine : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverScoreText;

    public GameObject spawnPointPC;
    public GameObject spawnPointVR;

    public float scoreIncrementInterval = 1.0f;
    private float timer;
    private ulong score;
    private bool isGameOver;
    private bool hasGameStarted;

    private PhotonView photonView;

    void Start()
    {
        photonView = PhotonView.Get(this);
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

    public void RestartGame()
    {
        if (PhotonNetwork.IsMasterClient)
            GameObject.Find("GroundSectionSpawner").GetComponent<GroundSectionSpawner>().ResetAll();
        photonView.RPC("RPC_RestartGame", RpcTarget.All);
    }

    public void LoadColorMiniGame()
    {
        photonView.RPC("RPC_LoadColorMiniGame", RpcTarget.All);
    }

    [PunRPC]
    private void RPC_LoadColorMiniGame()
    {
        PhotonNetwork.LoadLevel("ColorMiniGame");
    }

    [PunRPC]
    private void RPC_RestartGame()
    {
        isGameOver = false;
        gameOverPanel.SetActive(false);
        score = 0;

        GameObject playerVR = GameObject.Find("PlayerVR(Clone)");
        playerVR.transform.position = spawnPointVR.transform.position;
        playerVR.GetComponent<Navigation>().ResetAll();
        GameObject.Find("Player(Clone)").transform.position = spawnPointPC.transform.position;
    }
}