using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(1); // 피해량을 원하는 값으로 설정
                Destroy(gameObject); // 발사체 파괴
            }
        }
        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }


}
