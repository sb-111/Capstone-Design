using UnityEngine;
using Photon.Pun;
using System.Collections;

public class TerrainPortal : MonoBehaviour
{
    public Transform receiver;

    private bool playerIsOverlapping = false;
    private Transform overlappingPlayer = null; 

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && other.gameObject.GetComponent<PhotonView>().IsMine)
        {
            playerIsOverlapping = true;
            overlappingPlayer = other.transform;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && other.gameObject.GetComponent<PhotonView>().IsMine)
        {
            playerIsOverlapping = false;
            overlappingPlayer = null;
        }
    }

    void Update()
    {
        if (playerIsOverlapping && overlappingPlayer != null)
        {
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
        }
    }

    private IEnumerator DisableColliderTemporarily(Collider collider, float delay)
    {
        collider.enabled = false; 
        yield return new WaitForSeconds(delay); 
        collider.enabled = true; 
    }
}