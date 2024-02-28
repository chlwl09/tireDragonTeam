using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Resources;

public class Life : MonoBehaviour
{
    private Player player;
    public TextMeshProUGUI healthText;

    void Start()
    {
        player = FindObjectOfType<Player>();
        // Life 스크립트가 부착된 GameObject에 연결된 Player 스크립트를 찾음

        if (Player.Instance.health == null)
        {
            Debug.LogError("Player 스크립트를 찾을 수 없습니다.");
        }
    }

    void Update()
    {
        // 이후 PlayerHP 변수를 사용할 수 있음
        healthText.text = " HP : " +  player.health.ToString();
    }
}
