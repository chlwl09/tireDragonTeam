using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player Instance;

    public GameObject BulletPrefabs; // 발사체 프리팹
    public Transform BulletSpawnPoint; // 발사체 생성 위치
    public Color hitColor = Color.gray;    // 맞았을 때의 색상
    public float hitDuration = 0.5f;        // 맞았을 때 색상이 유지되는 시간
    public AudioClip hitSound;             // 맞았을 때의 오디오 소리

    public Vector2 inputVec;
    public float MoveSpeed = 1.0f;
    public int health = 3; // 플레이어 생명력
    public int bullteSpeed = 10;

    //boss HP UI
    public GameObject HPbar;
    public Canvas canvas;
    public RectTransform hpbarTransform;

    public Slider healthSlider; //UI의 HP 수치 건드는 
    public int Maxhealth;//몬스터가 생성되자마자의 HP를 저장하는 함수

    AudioManager audioManager;   // 오디오 매니저
    Rigidbody2D rigid;
    private Color originalColor;           // 원래의 색상
    private SpriteRenderer spriteRenderer; // 스프라이트 렌더러

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Instance = this;
        originalColor = spriteRenderer.color;
        // AudioManager 컴포넌트를 가져오거나 추가합니다.
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found. Make sure AudioManager script is attached to an object in the scene.");
        }
    }
    public void Start()
    {
        Maxhealth = health;
    }

    void Update()
    {
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");

        // 스페이스바를 누르면 발사체 발사
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FireProjectile();
        }

    }

    private void FixedUpdate()
    {
        Vector2 nextVc = inputVec * MoveSpeed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVc);
        UpdateHealthUI();

    }
    void FireProjectile()
    {  // 발사체를 생성하고 오른쪽으로 발사
        if (BulletPrefabs != null && BulletSpawnPoint != null)
        {
            GameObject projectile = Instantiate(BulletPrefabs, BulletSpawnPoint.position, Quaternion.identity);

            Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
            projectileRb.velocity = Vector3.right * bullteSpeed; // 발사체의 초기 속도를 설정 (오른쪽으로 10의 속도로 발사)
            if (audioManager != null)
            {
                audioManager.PlayShootSound();
            }

        }
        else
        {
            Debug.LogError("발사체 프리팹 또는 발사체 생성 위치가 설정되지 않았습니다. Inspector에서 설정하세요.");
        }
    }


    public void TakeDamage(int damage)
    {
        // 플레이어 생명력 감소
        health -= damage;

        // 생명력이 0 이하면 플레이어를 파괴하거나 다른 처리 수행
        if (health <= 0)
        {
            Destroy(gameObject);
            SceneManager.LoadScene("_G");
            // 또는 게임오버 처리 등을 수행할 수 있습니다.
        }
    }
    private void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            if (Maxhealth > 0)
            {
                healthSlider.normalizedValue = (float)health / (float)Maxhealth; // 현재 체력을 Slider 값으로 설정
                Debug.Log("HP 줄어듦");
            }
            else
            {
                Debug.LogError("Maxhealth는 0보다 커야 합니다.");
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        // 불렛에 맞았을 때
        if (other.CompareTag("Monster"))
        {
            // 플레이어의 색상 변경
            spriteRenderer.color = hitColor;

            // 맞은 소리 재생
            if (audioManager != null && hitSound != null)
            {
                audioManager.PlayHated();
            }

            // 일정 시간 후에 색상을 원래대로 되돌림
            Invoke("ResetColor", hitDuration);
        }
        void ResetColor()
        {
            spriteRenderer.color = originalColor;
        }

    }
}