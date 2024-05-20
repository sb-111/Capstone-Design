using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSCamera : MonoBehaviour
{
    //[SerializeField] private Transform cameraArm;
    [SerializeField] private Player target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float sensivity = 1.0f;
    [SerializeField] private float speed = 3.0f;
    float currentY = 0f;


    // 플레이어의 움직임이 끝나면 카메라 이동

    private void Start()
    {
        target = GetComponentInParent<Player>();
    }
    private void LateUpdate()
    {
        // -z축 방향으로 offset 크기만큼 떨어진 벡터 
        Vector3 direction = new Vector3(0, 0, -offset.magnitude);

        // 카메라의 수직 회전 값 = 타겟에서 계산해옴
        // 카메라의 수평 회전 값 = 타겟의 현재 y축 회전 값 
        Quaternion rotation = Quaternion.Euler(target.VRotation, target.transform.eulerAngles.y, 0);

        // 카메라의 position = 타겟의 위치 + 떨어진 거리벡터에 카메라 회전 적용
        transform.position = target.transform.position + rotation * direction;

        // 카메라가 항상 타겟을 바라보도록 설정
        transform.LookAt(target.transform.position + Vector3.up * offset.y);

    }

}
