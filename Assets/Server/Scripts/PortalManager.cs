using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PortalManager : MonoBehaviourPun
{
    [SerializeField] int currentHP = 100;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsBreak())
        {
            Break();
        }
    }

    void OnTriggerEnter(Collider coll)
    {
        Debug.Log("충돌");
        if (coll.tag == "Melee")
        {
            currentHP -= 10;
            Debug.LogWarning("무기충돌");

        }
    }
    private bool IsBreak()
    {
        return currentHP <= 0;
    }


    private void Break()
    {
        // 파괴
        GameManager.Instance.GameFinish();
        PhotonView PV = this.GetComponent<PhotonView>();
        if (PV.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
