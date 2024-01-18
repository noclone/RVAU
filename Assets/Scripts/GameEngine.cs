using TMPro;
using UnityEngine;

public class GameEngine : MonoBehaviour
{
    private ulong score;
    public TextMeshProUGUI scoreText;

    public float scoreIncrementInterval = 1.0f;
    private float timer;

    void Start()
    {
        score = 0;
        UpdateScoreText();
    }

    void Update()
    {
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
}