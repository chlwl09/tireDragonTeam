using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public float attackCooldown = 3f; // 각 공격 패턴 간격
    public float movementSpeed = 2f;  // 보스 몹의 움직임 속도
    public float bulletSpeed = 5f;     // 총알 이동 속도
    public float bulletAmplitude = 2f; // 사인 곡선의 높이 (진폭)


    float minY = -7f;          // 이동 가능한 최소 Y 위치
     float maxY = 6f;           // 이동 가능한 최대 Y 위치

    public GameObject bulletPrefab;    // 총알 프리팹
    public Transform bulletSpawnPoint; // 총알 발사 위치

    void Start()
    {
        // 코루틴 시작
        StartCoroutine(BossAttackCoroutine());
    }

    private void Update()
    {
        float newY = Mathf.PingPong(Time.time * movementSpeed, maxY - minY) + minY;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }


    IEnumerator BossAttackCoroutine()
    {
        while (true)
        {
            // 랜덤하게 패턴 선택
            // int randomPattern = Random.Range(1, 4); // 예제로 1부터 3까지의 패턴 중 하나를 랜덤 선택
            int randomPattern = 1;
            // 선택된 패턴 실행
            switch (randomPattern)
            {
                case 1:
                    AttackPattern1();
                    break;

                case 2:
                    AttackPattern2();
                    break;

                case 3:
                    AttackPattern3();
                    break;
            }

            // 다음 공격까지 대기
            yield return new WaitForSeconds(attackCooldown);
        }
    }

    void AttackPattern1()
    {

      
    }
        void AttackPattern2()
    {
        Debug.Log("공격 패턴 2 실행");
        // 패턴에 따른 동작 구현
    }

    void AttackPattern3()
    {
        Debug.Log("공격 패턴 3 실행");
        // 패턴에 따른 동작 구현
    }
}
