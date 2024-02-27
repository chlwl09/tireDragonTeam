using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 5f; // 몬스터 이동 속도
    public Transform player; // 플레이어의 Transform을 저장하기 위한 변수

    float maxMap = 10f;

    public enum diffEnemy
    {
        LineEnemy,
        FollowEnemy
    }

    public diffEnemy currentState;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        switch(currentState){

            // 일직선으로 가는 적
            case diffEnemy.LineEnemy:
                MoveLeftToRight();
                break;
            // 플레이어를 따라 가는 적
            case diffEnemy.FollowEnemy:
                FollowPlayer();
                break;

        }
    }

 

    void MoveLeftToRight()
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
        // 플레이어 방향으로 회전
        Vector2 direction = (player.position - transform.position).normalized;

        // 몬스터를 플레이어 쪽으로 이동
        transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);
    }
}
