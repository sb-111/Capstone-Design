using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalSpawnerManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void OnTriggerEnter(Collider coll)
    {
        Debug.Log("충돌");
        if (coll.tag == "Melee")
        {
            Countdown.mode = 1;
            GameManager.portalOwner = true;
            SpawnManager.Instance.portalSpawn();
            Debug.Log("카운트 다운 변경");
            Destroy(gameObject);
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
