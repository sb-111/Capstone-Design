using UnityEngine;
using Photon.Pun; 

public class TerrainPortal : MonoBehaviour
{
    public Transform player;
    public Transform receiver;

    private bool playerIsOverlapping = false;

    void Start() 
    {
        FindLocalPlayer();
    }

    void FindLocalPlayer()
    {
        GameObject localPlayerObj = null;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (obj.GetComponent<PhotonView>().IsMine)
            {
                localPlayerObj = obj;
                break;
            }
        }
        if (localPlayerObj != null)
        {
            player = localPlayerObj.transform;
        }
        else
        {
            Debug.LogWarning("로컬 플레이어를 찾을 수 없습니다.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && other.gameObject.GetComponent<PhotonView>().IsMine)
        {
            playerIsOverlapping = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && other.gameObject.GetComponent<PhotonView>().IsMine)
        {
            playerIsOverlapping = false;
        }
    }

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
