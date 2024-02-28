using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip shootSound; // 발사 소리 AudioClip
    public AudioClip enemyHitSound;   // 적이 맞았을 때 소리 AudioClip
    public AudioClip hateSound; // 맞은 소리

    private AudioSource audioSource;

    void Start()
    {
        // AudioSource 컴포넌트를 가져오거나 추가합니다.
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // 발사 소리를 재생하는 함수
    public void PlayShootSound()
    {
        if (shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
    }
    // 적이 맞았을 때 소리를 재생하는 함수
    public void PlayEnemyHitSound()
    {
        if (enemyHitSound != null)
        {
            audioSource.PlayOneShot(enemyHitSound);
        }
    } 
    public void PlayHated()
    {
        if (enemyHitSound != null)
        {
            audioSource.PlayOneShot(hateSound);
        }
    }
}
