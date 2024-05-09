using UnityEngine;

public class TerrainPortal : MonoBehaviour
{
    public Transform player;
    public Transform receiver; // 다른 포탈의 위치

    private bool playerIsOverlapping = false;

    // 트리거 콜라이더에 무언가 진입하면 호출됩니다.
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerIsOverlapping = true;
        }
    }

    // 트리거 콜라이더에서 무언가 나가면 호출됩니다.
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerIsOverlapping = false;
        }
    }

    // 매 프레임마다 호출됩니다.
    void Update()
    {
        if (playerIsOverlapping)
        {
            Vector3 portalToPlayer = player.position - transform.position;
            float dotProduct = Vector3.Dot(transform.up, portalToPlayer);

            // 플레이어가 포탈을 통과했는지 확인
            if (dotProduct < 0f)
            {
                // 플레이어를 다른 포탈의 위치로 이동
                float rotationDiff = -Quaternion.Angle(transform.rotation, receiver.rotation);
                rotationDiff += 180;
                player.Rotate(Vector3.up, rotationDiff);

                Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToPlayer;
                player.position = receiver.position + positionOffset;

                playerIsOverlapping = false;
            }
        }
    }
}