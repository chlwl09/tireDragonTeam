using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject[] monsterPrefabs; // 다양한 몬스터 프리팹 배열
    public Transform[] spawnPoints; // 몬스터가 나올 위치 배열

     int score;
     int randomMonsterIndex;
    int spawnInterval = 3;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        InvokeRepeating("SpawnRandomMonster", 0f, spawnInterval);       
    }
    private void Update()
    {
        
    }
    void SpawnRandomMonster()
    {
        score = Score.Instance.score;

        if (score < 1000)
        {
             randomMonsterIndex = 0;
            spawnInterval = 5;
        }
        else if (score < 3000)
        {
            randomMonsterIndex = Random.Range(0, 2);

            spawnInterval = 4;
        }
        else if(score < 7000 )
        {
            randomMonsterIndex = Random.Range(0, 3);
            spawnInterval = 3 ;
        }
        else if (score > 10000)
        {
            SceneManager.LoadScene("_2");
        }
       
        


        // 랜덤으로 몬스터와 위치 선택
        //int randomMonsterIndex = Random.Range(0, monsterPrefabs.Length);
        int randomSpawnPointIndex = Random.Range(0, spawnPoints.Length);

            
        // 선택된 몬스터와 위치에 몬스터 생성
       Instantiate(monsterPrefabs[randomMonsterIndex], spawnPoints[randomSpawnPointIndex].position, Quaternion.identity);

    }







}
