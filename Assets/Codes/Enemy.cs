using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static Enemy Instance;

    public float moveSpeed = 5f; // 몬스터 이동 속도
     Transform player; // 플레이어의 Transform을 저장하기 위한 변수
    public int health = 3; // 몬스터 생명력
    public GameObject projectilePrefab; // 발사체 프리팹
    public Transform projectileSpawnPoint; // 발사체 생성 위치
    public float fireInterval = 2f; // 발사 간격
    public  int m1Score = 50;
    
    private float nextFireTime = 0f; // 다음 발사 시간
    public float maxMap = 10f;


    private void Awake()
    {
    }
    public enum diffEnemy
    {
        LineEnemy,
        FollowEnemy,
        fireEnemy
    }

    public diffEnemy currentState;

    private void Update()
    {
    }

    private void FixedUpdate()
    {
        switch (currentState)
        {

            // 일직선으로 가는 적
            case diffEnemy.LineEnemy:
                MoveRightToLeft();
                break;

            // 플레이어를 따라 가는 적
            case diffEnemy.FollowEnemy:
                FollowPlayer();
                break;
            // 제자리에서 발사만 하는 적
            case diffEnemy.fireEnemy:
                if (Time.time > nextFireTime)
                {
                    FireProjectile();
                    nextFireTime = Time.time + fireInterval;
                }
               
                break;

        }
    }



    void MoveRightToLeft()
    {
        // 몬스터를 오른쪽으로 이동
        transform.Translate(Vector3.left * moveSpeed * Time.fixedDeltaTime);

        // 만약 몬스터가 화면 왼쪽으로 벗어나면 초기 위치로 이동
        if (transform.position.x < -maxMap) // 이동할 최대 x 좌표를 설정하십시오.
        {
            ResetPosition();
        }
    }

    void ResetPosition()
    {
        // 몬스터를 초기 위치로 되돌리기
        transform.position = new Vector3(maxMap, transform.position.y, transform.position.z);
    }
    void FollowPlayer()
    {
        player = Player.Instance.transform;
        // 플레이어 방향으로 회전
        Vector2 direction = (player.position - transform.position).normalized;

        if(player.transform.position.x > gameObject.transform.position.x) 
        {
            MoveRightToLeft();
        }
        else
        {
            // 몬스터를 플레이어 쪽으로 이동
            transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);
        }

      
    }
    public void TakeDamage(int damage)
    {
        // 몬스터 생명력 감소
        health -= damage;

        // 생명력이 0 이하면 몬스터를 파괴하거나 다른 처리 수행
        if (health <= 0)
        {
            OnMonsterDestroyed();
            Destroy(gameObject);
            
            // 또는 다른 처리를 수행할 수 있습니다.
        }
    }
    void FireProjectile()
    {
        // 발사체를 생성하고 왼쪽으로 발사
        if (projectilePrefab != null && projectileSpawnPoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.Euler(0, 0, 90f));
            Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
            projectileRb.velocity = new Vector2(-10f, 0f); // 발사체의 초기 속도를 설정 (왼쪽으로 10의 속도로 발사)
        }
        else
        {
            Debug.LogError("발사체 프리팹 또는 발사체 생성 위치가 설정되지 않았습니다. Inspector에서 설정하세요.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Invoke("DeadPlayer", 1f);
    }
    void DeadPlayer(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(1); // 피해량을 원하는 값으로 설정
                Destroy(gameObject);
            }
        }
    }
    void OnMonsterDestroyed()
    {
        // 몬스터 파괴 이벤트를 발생시킴
        if (MonsterDestroyedEvent != null)
        {
            MonsterDestroyedEvent(this, m1Score); // 몬스터 인스턴스와 점수를 파라미터로 전달
        }
    }

    // 몬스터 파괴 이벤트를 수신할 델리게이트 및 이벤트
    public delegate void MonsterDestroyedEventHandler(Enemy monster, int score);
    public static event MonsterDestroyedEventHandler MonsterDestroyedEvent;

    
}

