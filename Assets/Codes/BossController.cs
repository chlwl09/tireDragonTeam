using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    AudioManager audioManager; // 오디오 매니저

    public float attackCooldown = 3f; // 각 공격 패턴 간격
    public float movementSpeed = 2f;  // 보스 몹의 움직임 속도
    public float bulletSpeed = 5f;     // 총알 이동 속도
    public float currentAngle = 90f; // 현재 발사 각도

    public float maxHealth = 100f;     // 보스의 최대 체력
    public float currentHealth;        // 현재 체력


    float minY = -7f;          // 이동 가능한 최소 Y 위치
     float maxY = 6f;           // 이동 가능한 최대 Y 위치

    public GameObject bulletPrefab;    // 총알 프리팹
    public GameObject bulletPrefab1;    // 총알 프리팹
    public Transform bulletSpawnPoint; // 총알 발사 위치
    public Transform bulletSpawnPointUp; // 총알 발사 위치
    public Transform bulletSpawnPointDown; // 총알 발사 위치

    //BossHPUI
    public GameObject HPbar;
    public Canvas canvas;
    public RectTransform hpbarTransform;

    public Slider healthSlider; //UI의 HP 수치 건드는 



    void Start()
    {
        currentHealth = maxHealth; // 시작 시 현재 체력을 최대 체력으로 초기화
        // 코루틴 시작
        StartCoroutine(BossAttackCoroutine());

        UpdateHealthUI();

        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found. Make sure AudioManager script is attached to an object in the scene.");
        }

        healthSlider = hpbarTransform.GetComponent<Slider>();
    }

    private void Update()
    {
        float newY = Mathf.PingPong(Time.time * movementSpeed, maxY - minY) + minY;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        UpdateHealthUI();
    }

    IEnumerator BossAttackCoroutine()
    {
        while (true)
        {
            // 랜덤하게 패턴 선택
             int randomPattern = Random.Range(1, 3); // 예제로 1부터 3까지의 패턴 중 하나를 랜덤 선택
            // int randomPattern = 2;
            // 선택된 패턴 실행
            AttackPattern1();
            if(currentHealth < 50)
            {
                switch(randomPattern) 
                {
                    case 1:
                        AttackPattern2();
                        break;
                    case 2:
                        AttackPattern3();
                        break;
                }

            }

            // 다음 공격까지 대기
            yield return new WaitForSeconds(attackCooldown);
        }
    }

    void AttackPattern1()
    {
        Debug.Log("일직선으로 총알 공격 실행");    

        // 총알 발사 위치
        Vector3 spawnPosition = bulletSpawnPoint.position;

        // 총알 생성 및 설정
        GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);

        // 총알의 초기 이동 방향 (여기에서는 왼쪽으로 발사)
        Vector3 bulletDirection = Vector3.left;

        // 총알에 초기 이동 방향과 속도 설정
        bullet.GetComponent<Rigidbody2D>().velocity = bulletDirection * bulletSpeed;

        // 총알의 회전 코루틴 시작
        StartCoroutine(RotateAndShoot(bullet, currentAngle)); // 여기에 원하는 각도의 절반을 지정하세요
    }

    IEnumerator RotateAndShoot(GameObject bullet, float maxRotationAngle)
    {
        float elapsedTime = 0f;

        while (bullet != null)
        {
            // 현재 각도 (PingPong 함수를 사용하여 값이 0부터 maxRotationAngle까지 반복)
            float currentRotation = Mathf.PingPong(elapsedTime * 2f, maxRotationAngle);

            // 회전 적용
            bullet.transform.rotation = Quaternion.Euler(0, 0, currentRotation);

            // 각도에 따라 발사 방향 설정
            Vector3 bulletDirection = Quaternion.Euler(0, 0, currentRotation) * Vector3.left;

            // 총알에 이동 방향과 속도 설정
            bullet.GetComponent<Rigidbody2D>().velocity = bulletDirection * bulletSpeed;

            // 경과 시간 업데이트
            elapsedTime += Time.deltaTime;

            yield return null;
        }
    }



    void AttackPattern2()
    {
        Debug.Log("양갈래로 총알 공격 실행");

        // 총알 발사 위치
        Vector3 spawnPosition = bulletSpawnPointUp.position;

        // 총알 생성 및 설정
        GameObject bulletUp = Instantiate(bulletPrefab1, spawnPosition, Quaternion.identity);
        GameObject bulletDown = Instantiate(bulletPrefab1, spawnPosition, Quaternion.identity);

        // 총알의 초기 이동 방향 (왼쪽 위, 왼쪽 아래로 발사)
        Vector3 bulletDirectionUp = (Vector3.left + Vector3.up).normalized; // 왼쪽 위 대각선
        Vector3 bulletDirectionDown = (Vector3.left - Vector3.up).normalized; // 왼쪽 아래 대각선

        // 총알에 초기 이동 방향과 속도 설정
        bulletUp.GetComponent<Rigidbody2D>().velocity = bulletDirectionUp * bulletSpeed;
        bulletDown.GetComponent<Rigidbody2D>().velocity = bulletDirectionDown * bulletSpeed;

        // 총알의 회전 코루틴 시작
        StartCoroutine(RotateAndShoot(bulletUp, -15f)); // 왼쪽 위로 발사하면서 회전
        StartCoroutine(RotateAndShoot(bulletDown, 15f)); // 왼쪽 아래로 발사하면서 회전

   

    }

    void AttackPattern3()
    {
        Debug.Log("양갈래로 총알 공격 실행");

        // 총알 발사 위치
        Vector3 spawnPosition = bulletSpawnPointDown.position;

        // 총알 생성 및 설정
        GameObject bulletUp = Instantiate(bulletPrefab1, spawnPosition, Quaternion.identity);
        GameObject bulletDown = Instantiate(bulletPrefab1, spawnPosition, Quaternion.identity);

        // 총알의 초기 이동 방향 (왼쪽 위, 왼쪽 아래로 발사)
        Vector3 bulletDirectionUp = (Vector3.left + Vector3.up).normalized; // 왼쪽 위 대각선
        Vector3 bulletDirectionDown = (Vector3.left - Vector3.up).normalized; // 왼쪽 아래 대각선

        // 총알에 초기 이동 방향과 속도 설정
        bulletUp.GetComponent<Rigidbody2D>().velocity = bulletDirectionUp * bulletSpeed;
        bulletDown.GetComponent<Rigidbody2D>().velocity = bulletDirectionDown * bulletSpeed;

        // 총알의 회전 코루틴 시작
        StartCoroutine(RotateAndShoot(bulletUp, -15f)); // 왼쪽 위로 발사하면서 회전
        StartCoroutine(RotateAndShoot(bulletDown, 15f)); // 왼쪽 아래로 발사하면서 회전

        
    }

    // 보스가 데미지를 받는 함수
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (audioManager != null)
        {
            audioManager.PlayEnemyHitSound();
        }


        // 체력이 0 이하로 떨어지면 보스를 처치
        if (currentHealth <= 0)
        {
            currentHealth = 0; // 체력이 음수가 되지 않도록 보정
            UpdateHealthUI();
            SceneManager.LoadScene("_4");
            Debug.Log("보스 처치!");
            // 여기에서 추가적인 처리를 할 수 있습니다.
        }
        else
        {
            Debug.Log("보스 현재 체력: " + currentHealth);
        }
    }

    //보스 HP UI
    private void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            if (maxHealth > 0)
            {
                healthSlider.normalizedValue = (float)currentHealth / (float)maxHealth; // 현재 체력을 Slider 값으로 설정
                Debug.Log("HP 줄어듦");
            }
            else
            {
                Debug.LogError("Maxhealth는 0보다 커야 합니다.");
            }
        }
    }
}
