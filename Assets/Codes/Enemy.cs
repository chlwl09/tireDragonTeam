using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public SpriteRenderer renderer; //SpriteRenderer 사용을 위한 불러옴
    private Color originalColor;//첫 컬러를 저장하기 위한 변수
    public Color hitColor = Color.red;//hit 판정 시 나오는 컬러
    public float hitDuration = 0.1f;//시간 임의

    private float nextFireTime = 0f; // 다음 발사 시간
    public float maxMap = 10f;

    //몬스터 HPUI 구현
    public GameObject HPbar;
    public Canvas canvas;
    public RectTransform hpbarTransform;
    public float height = -1f;

    public Slider healthSlider; //UI의 HP 수치 건드는 
    public int Maxhealth;//몬스터가 생성되자마자의 HP를 저장하는 함수
    

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

    public void Start()
    {
        renderer = GetComponent<SpriteRenderer>();//SpriteRenderer 사용을 위해  
        originalColor = renderer.color; //첫 컬러 저장

        Maxhealth = health;



        UpdateHealthUI();

        canvas = GameObject.FindWithTag("Canvas").GetComponent<Canvas>();

        if (canvas != null)
        {
            hpbarTransform = Instantiate(HPbar, canvas.transform).GetComponent<RectTransform>(); // HPBar 위치 저장
        }
        else
        {
            Debug.LogError("Canvas를 찾을 수 없습니다!");
        }

        healthSlider = hpbarTransform.GetComponent<Slider>();
    }

    private void Update()
    {
        if (hpbarTransform != null)
        {
            Vector3 hpbarPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + height, 0));
            hpbarTransform.position = hpbarPos;
        }
       
    }

    private void FixedUpdate()
    {

        UpdateHealthUI();
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
            if (hpbarTransform != null)
            {
                Destroy(hpbarTransform.gameObject);
            }

            // 또는 다른 처리를 수행할 수 있습니다.
        }
    }
    void FireProjectile()
    {
        // 발사체를 생성하고 왼쪽으로 발사
        if (projectilePrefab != null && projectileSpawnPoint != null)
        {
            Vector3 updateVec = projectileSpawnPoint.position + new Vector3(-1, 0, 0);
            GameObject projectile = Instantiate(projectilePrefab, updateVec , Quaternion.Euler(0, 0, 0));
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

        if (other.CompareTag("Player"))
        {
            StartCoroutine(HitAnima(hitColor, hitDuration));

            // HP바를 제거합니다.
            if (hpbarTransform != null)
            {
                Destroy(hpbarTransform.gameObject);
            }
        }
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

    IEnumerator HitAnima(Color color, float duration)
    {
        renderer.color = color;
        yield return new WaitForSeconds(duration);
        renderer.color = originalColor;
    }
        
    private void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            if (Maxhealth > 0)
            {
                healthSlider.normalizedValue= (float)health / (float)Maxhealth; // 현재 체력을 Slider 값으로 설정
                Debug.Log("HP 줄어듦");
            }
            else
            {
                Debug.LogError("Maxhealth는 0보다 커야 합니다.");
            }
        }
    }

    // 몬스터 파괴 이벤트를 수신할 델리게이트 및 이벤트
    public delegate void MonsterDestroyedEventHandler(Enemy monster, int score);
    public static event MonsterDestroyedEventHandler MonsterDestroyedEvent;

    
}

