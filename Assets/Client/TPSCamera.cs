using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSCamera : MonoBehaviour
{
    [SerializeField] private Transform cameraArm;
    [SerializeField] private Transform player;
    [SerializeField] private float sensivity = 1.0f;
    [SerializeField] private float speed = 3.0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MouseRotate();
        Move();
    }

    private void Move()
    {
        float inputX = Input.GetAxis("Horizontal"); // x축 이동(-1/1)
        float inputZ = Input.GetAxis("Vertical"); // z축 이동(-1/1)

        Vector3 moveVec = new Vector3(inputX, 0, inputZ);
        bool isMove = moveVec.magnitude != 0;

        if (isMove)
        {
            // y축 성분 제거된 카메라의 z축, x축 방향벡터
            Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
            Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
            
            Vector3 moveDir = lookForward * moveVec.z + lookRight * moveVec.x;

            player.forward = lookForward;
            transform.position += moveDir * speed * Time.deltaTime;
        }

    }

    private void MouseRotate()
    {
        float mouseValueX = Input.GetAxis("Mouse X");
        float mouseValueY = Input.GetAxis("Mouse Y");
        
        Debug.Log($"mouseX: {mouseValueX}, mouseY: {mouseValueY}");

        Vector3 cameraAngle = cameraArm.rotation.eulerAngles; // 오일러값

        float cameraX = cameraAngle.x - (mouseValueY * sensivity); // x축 회전용(수직)
        float cameraY = cameraAngle.y + (mouseValueX * sensivity); // y축 회전용(수평)
        float cameraZ = cameraAngle.z;
        // 90~0(아래)~360~270(위)
        if(cameraX > 180f) 
        {
            // 수직 각도 제한(위)
            cameraX = Mathf.Clamp(cameraX, 300f, 360f);
        }
        else
        {
            // 수평 각도 제한(아래)
            cameraX = Mathf.Clamp(cameraX, -1f, 90f);
        }
        Debug.Log($"cameraX: {cameraX}");

        // 참고)실제 회전은 rotation에서 Quaternion 값을 이용한다
        // Quaternion.Euler : 오일러 -> 쿼터니언
        cameraArm.rotation = Quaternion.Euler(cameraX, cameraY, cameraZ);

        player.rotation = Quaternion.Euler(0, cameraArm.rotation.eulerAngles.y, 0); // 카메라 방향으로 캐릭터 회전

    }
}
