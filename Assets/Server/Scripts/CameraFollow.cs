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
    bool isSpawn = false;

    public void SetPlayer(GameObject clone)
    {
        playerObject = clone;
        target = playerObject.GetComponent<Player>();
        PhotonView cameraView = playerObject.GetComponent<PhotonView>();
        if (cameraView.IsMine)
        {
            isFollowing = true;
            isDie = false;
            isSpawn = false;
        }
        else
            Debug.LogWarning("�÷��̾� �� ã��");
    }

    // Start is called before the first frame update
    void Start()
    {
      
     
        
      
    }
    void Update()
    {

        if (target==null&&!isSpawn)
        {
            isDie = true;
            isSpawn = true;
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
        // -z�� �������� offset ũ�⸸ŭ ������ ���� 
        Vector3 direction = new Vector3(0, 0, -offset.magnitude);

        // ī�޶��� ���� ȸ�� �� = Ÿ�ٿ��� ����ؿ�
        // ī�޶��� ���� ȸ�� �� = Ÿ���� ���� y�� ȸ�� �� 
        Quaternion rotation = Quaternion.Euler(target.VRotation, target.transform.eulerAngles.y, 0);

        // ī�޶��� position = Ÿ���� ��ġ + ������ �Ÿ����Ϳ� ī�޶� ȸ�� ����
        transform.position = target.transform.position + rotation * direction;

        // ī�޶� �׻� Ÿ���� �ٶ󺸵��� ����
        transform.LookAt(target.transform.position + Vector3.up * offset.y);
    }
    }


}
