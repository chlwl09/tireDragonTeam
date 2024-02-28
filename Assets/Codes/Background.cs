using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public float scrollSpeed = 2.0f;   // 배경 이동 속도
    public float resetPosition = -40.0f; // 배경의 X값이 이 값보다 작아지면 다시 특정 위치로 돌아감
    public float resetTarget = 40.0f;    // 배경이 돌아갈 특정 위치의 X값

    void FixedUpdate()
    {
        // 배경을 이동시킴
        transform.Translate(Vector3.left * scrollSpeed * Time.fixedDeltaTime);

        // 배경의 X값이 특정 값보다 작아지면 다시 특정 위치로 돌아감
        if (transform.position.x <= resetPosition)
        {
            // 배경을 특정 위치로 이동시킴
            Vector3 newPos = new Vector3(resetTarget, transform.position.y, transform.position.z);
            transform.position = newPos;

            Debug.Log(transform.position.x);
        }
    }
}
