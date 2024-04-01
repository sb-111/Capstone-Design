using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    // Start is called before the first frame update
    bool portalOwner = false;
    void Start()
    {
        
    }
    void OnTriggerEnter(Collider coll)
    {
        Debug.Log("충돌");
        if (coll.gameObject.tag == "PortalSpawner")
        {
            Countdown.mode = 1;
            Destroy(coll.gameObject);
            portalOwner = true;
            SpawnManager.Instance.portalSpawn();
            Debug.Log("카운트 다운 변경");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
