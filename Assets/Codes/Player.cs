using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    public GameObject BulletPrefabs; // 발사체 프리팹
    public Transform BulletSpawnPoint; // 발사체 생성 위치

    public Vector2 inputVec;
    public float MoveSpeed = 1.0f;
    public int health = 3; // 플레이어 생명력


    Rigidbody2D rigid;
    SpriteRenderer spriter;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter= GetComponent<SpriteRenderer>();
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

        spriter.flipX = inputVec.x <0 ;
    }

    private void FixedUpdate()
    {
        Vector2 nextVc = inputVec * MoveSpeed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVc);
    }
    void FireProjectile()
    {  // 발사체를 생성하고 오른쪽으로 발사
        if (BulletPrefabs != null && BulletSpawnPoint != null)
        {
            GameObject projectile = Instantiate(BulletPrefabs, BulletSpawnPoint.position, Quaternion.Euler(0, 0, 90f));

            Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
            projectileRb.velocity = Vector3.right * 10f; // 발사체의 초기 속도를 설정 (오른쪽으로 10의 속도로 발사)
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
            // 또는 게임오버 처리 등을 수행할 수 있습니다.
        }
    }
   

}