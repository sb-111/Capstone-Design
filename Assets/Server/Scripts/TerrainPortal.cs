using UnityEngine;
using Photon.Pun;
using System.Collections;

public class TerrainPortal : MonoBehaviour
{
    public Transform receiver;

    private bool playerIsOverlapping = false;
    private Transform overlappingPlayer = null;
    private GameObject player;
    private Player players;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            players = other.gameObject.GetComponent<Player>();
            playerIsOverlapping = true;
            overlappingPlayer = other.transform;
            GameObject obj = other.gameObject;
            if (obj.transform.parent != null)
                player = obj.transform.parent.gameObject;
            else player = obj;
            Debug.Log("포탈 테스트");
        }
    }

    void OnTriggerExit(Collider other)
    {
       
        if (other.tag == "Player")
        {
            playerIsOverlapping = false;
            overlappingPlayer = null;
            player = null;
            players = null;
        }
    }



    void Update()
    {
        if (playerIsOverlapping && overlappingPlayer != null)
        {
            Vector3 portalToPlayer = overlappingPlayer.position - transform.position;
            float dotProduct = Vector3.Dot(transform.up, portalToPlayer);

            Debug.Log("포탈 테스트" + dotProduct);
            if (dotProduct <= 0f)
            {
                StartCoroutine(DisableColliderTemporarily(receiver.GetComponent<Collider>(), 3f));
                float time = 0f;
                while (time < 0.5f)
                {
                    time += Time.deltaTime;
                    //player.transform.position = receiver.position;
                    GameManager.Instance.TeleportPlayer(receiver, player);
                    // photonVIew.RPC("TeleportPlayer", RpcTarget.All, receiver.position);
                }
        
                playerIsOverlapping = false;
                overlappingPlayer = null;
                Debug.Log("포탈 테스트 들어가지나" + player.transform.position + receiver.position);
            }
            /*
            Vector3 portalToPlayer = overlappingPlayer.position - transform.position;
            float dotProduct = Vector3.Dot(transform.up, portalToPlayer);


            if (dotProduct < 0f)
            {

                float rotationDiff = -Quaternion.Angle(transform.rotation, receiver.rotation);
                rotationDiff += 180;
                overlappingPlayer.Rotate(Vector3.up, rotationDiff);

                Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToPlayer;
                overlappingPlayer.position = receiver.position + positionOffset;


                playerIsOverlapping = false;
                overlappingPlayer = null;


                StartCoroutine(DisableColliderTemporarily(receiver.GetComponent<Collider>(), 3f));
            }
            */
        }
    }
    
    
    private IEnumerator DisableColliderTemporarily(Collider collider, float delay)
    {
        collider.enabled = false; 
        yield return new WaitForSeconds(delay); 
        collider.enabled = true; 
    }
}