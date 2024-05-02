using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class CameraFollow : MonoBehaviourPunCallbacks
{
    //[SerializeField] private Transform cameraArm;

    [SerializeField] private Vector3 offset;
    private GameObject playerObject;
    [SerializeField] private float sensivity = 1.0f;
    [SerializeField] private float speed = 3.0f;
    float currentY = 0f;
    Player target;
    bool isFollowing = false;

    bool isDie = false;


    public void SetPlayer(GameObject clone)
    {
        playerObject = clone;
        target = playerObject.GetComponent<Player>();
        PhotonView cameraView = playerObject.GetComponent<PhotonView>();
        if (cameraView.IsMine)
        {
            isFollowing = true;
            isDie = false;
        }
        else
            Debug.LogWarning("플레이어 못 찾음");
    }

    // Start is called before the first frame update
    void Start()
    {
      
     
        
      
    }
    void Update()
    {

        if (target==null)
        {
            isDie = true;
            GameManager.Instance.PlayerDead();
        }
    }
    private void LateUpdate()
    {
    if (isDie)
     {
            return;
     }
    else if (isFollowing) {
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


}
