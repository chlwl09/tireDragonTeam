using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    private int score = 0;
    private float timer = 0f;
    private float scoreIncreaseInterval = 1f;

    private void Update()
    {
        //타이머를 업데이트하여 시간을 축적
        timer += Time.deltaTime;

        // scoreIncreaseInterval마다 점수를 증가시킴
        if (timer >= scoreIncreaseInterval)
        {
            score++;
            UpdateScoreText();
            timer = 0f; // 타이머 초기화
        }
    }

    public void IncreaseScore(int points)
    {
        score += points;
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        scoreText.text = score.ToString();
    }
}
