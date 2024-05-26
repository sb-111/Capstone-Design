using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CreatePortal : MonoBehaviourPun
{
    public Vector3 raisePosition = new Vector3(0, 0, 0); 
    public float speed = 5f;
    public GameObject portal;
    public GameObject bomb;
    bool isRaising = false;
    private Collider[] allColliders;
    private Rigidbody[] allRigidbodies;
    // Start is called before the first frame update
    void Start()
    {
        allRigidbodies = this.GetComponentsInChildren<Rigidbody>();
        portal.gameObject.SetActive(false);
        bomb.gameObject.SetActive(false);
        //bomb.gameObject.SetActive(false);
        DisableAllColliders();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            //isRaising = true;
          // bomb.gameObject.SetActive(true);
        }

        if (isRaising)
        {
            portal.gameObject.SetActive(true);
            GameManager.Instance.DefenceStart();
            bomb.gameObject.SetActive(true);
            //PhotonNetwork.InstantiateRoomObject(bomb.name, transform.position, transform.rotation, 0);
            portal.transform.Translate(Vector3.up * speed * Time.deltaTime);

            if (portal.transform.position.y >= raisePosition.y)
            {
                isRaising = false;
                EnableAllColliders();
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter(Collider coll)
    {
        Debug.Log("충돌");
        if (coll.tag == "Melee")
        {
            isRaising = true;
          
          
            //PhotonView collPhotonView = coll.GetComponent<PhotonView>();
            PhotonView collPhotonView = coll.GetComponentInParent<PhotonView>();

            string playerID = collPhotonView.Owner.NickName;
            GameManager.Instance.GetPortal(playerID);


        }
    }
    void OnCollisionEnter(Collision col)
    {
        Debug.Log("충돌");
        if (col.gameObject.CompareTag("Player"))
        {
            isRaising = true;
        }
    }
    void DisableAllColliders()
    {
        foreach (Rigidbody rb in allRigidbodies)
        {
            rb.isKinematic = true;
        }
    }

 
    void EnableAllColliders()
    {
        foreach (Rigidbody rb in allRigidbodies)
        {
            rb.isKinematic = false;
        }

      
    }


}
