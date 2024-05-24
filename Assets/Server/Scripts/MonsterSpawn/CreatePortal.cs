using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePortal : MonoBehaviour
{
    public Vector3 raisePosition = new Vector3(0, 0, 0); 
    public float speed = 5f;
    public GameObject portal;
    bool isRaising = false;
    private Collider[] allColliders;
    private Rigidbody[] allRigidbodies;
    // Start is called before the first frame update
    void Start()
    {
        allRigidbodies = this.GetComponentsInChildren<Rigidbody>();
        portal.gameObject.SetActive(false);
        DisableAllColliders();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            isRaising = true;
        }

        if (isRaising)
        {
            portal.gameObject.SetActive(true);


            portal.transform.Translate(Vector3.up * speed * Time.deltaTime);

            if (portal.transform.position.y >= raisePosition.y)
            {
                isRaising = false;
                EnableAllColliders();
                Destroy(gameObject);
            }
        }
    }

    void OnCollisionEnter(Collision col)
    {
        Debug.Log("Ãæµ¹");
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
