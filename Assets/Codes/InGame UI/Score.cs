using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public static Score Instance { get; private set; }

    public TextMeshProUGUI scoreText;
    public int score = 0;
    private float timer = 0f;
    private float scoreIncreaseInterval = 1f;

    private void Awake()
    {
        Instance = this;
        Enemy.MonsterDestroyedEvent += OnMonsterDestroyed;
    }

    private void Update()
    {
        //타이머를 업데이트하여 시간을 축적
        timer += Time.deltaTime;

        // scoreIncreaseInterval마다 점수를 증가시킴
        if (timer >= scoreIncreaseInterval)
        {
            score += 10;
            UpdateScoreText();
            timer = 0f; // 타이머 초기화
        }
       
    }
    void OnMonsterDestroyed(Enemy monster, int scoreValue)
    {
        // 몬스터 파괴 이벤트 발생 시 호출되는 메소드
        IncreaseScore(scoreValue);
    }
    public void IncreaseScore(int points)
    {
        score += points;
        UpdateScoreText();  
    }

    void UpdateScoreText()
    {
        scoreText.text =  score.ToString();
    }
}
