using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubleTest : MonoBehaviour
{
    public float moveSpeed = 5f; // 이동 속도

    void Update()
    {
        // 수평과 수직 입력을 받아 이동 벡터를 계산합니다.
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * moveSpeed * Time.deltaTime;

        // 이동 벡터를 현재 위치에 더합니다.
        transform.Translate(movement);
    }
}
