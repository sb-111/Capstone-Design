using UnityEngine;
using Photon.Pun;

public class TerrainPortal : MonoBehaviour
{
    public Transform receiver;

    private bool playerIsOverlapping = false;
    private Transform overlappingPlayer = null; // 겹치는 플레이어를 저장할 변수

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && other.gameObject.GetComponent<PhotonView>().IsMine)
        {
            playerIsOverlapping = true;
            overlappingPlayer = other.transform; // 겹치는 플레이어의 Transform을 저장
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && other.gameObject.GetComponent<PhotonView>().IsMine)
        {
            playerIsOverlapping = false;
            overlappingPlayer = null; // 겹치는 플레이어 정보를 초기화
        }
    }

    void Update()
    {
        if (playerIsOverlapping && overlappingPlayer != null)
        {
            Vector3 portalToPlayer = overlappingPlayer.position - transform.position;
            float dotProduct = Vector3.Dot(transform.up, portalToPlayer);

            // 플레이어가 포탈을 통과했는지 확인
            if (dotProduct < 0f)
            {
                // 플레이어를 다른 포탈의 위치로 이동
                float rotationDiff = -Quaternion.Angle(transform.rotation, receiver.rotation);
                rotationDiff += 180;
                overlappingPlayer.Rotate(Vector3.up, rotationDiff);

                Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToPlayer;
                overlappingPlayer.position = receiver.position + positionOffset;

                playerIsOverlapping = false;
                overlappingPlayer = null; // 이동 후 겹치는 플레이어 정보 초기화
            }
        }
    }
}
