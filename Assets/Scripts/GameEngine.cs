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

    void Start()
    {
        score = 0;
        UpdateScoreText();
    }

    void Update()
    {
        if (!isGameOver)
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
}