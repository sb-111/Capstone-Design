using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class CreateStructure : MonoBehaviour
{
    public GameObject wall;
    Vector3 wallPos = new Vector3();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKey(M))
        {
            wallPos=transform.position+(0,0,1);
           
            PhotonNetwork.InstantiateRoomObject(wall.name, transform.position, transform.rotation, 0);
        }
        */
    }
    
}
