using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PortalManager : MonoBehaviourPun
{
    [SerializeField] int currentHP = 100;
    [SerializeField] GameObject[] enemies;
    [SerializeField] int fmax = 1;
    [SerializeField] int ftime = 5;
    [SerializeField] int smax = 3;
    [SerializeField] int stime = 5;
    [SerializeField] int tmax = 5;
    [SerializeField] int ttime = 3;
    public GameObject mot; 
    int mod = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable()
    {
        mot.gameObject.SetActive(false);
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].gameObject.SetActive(true);
            enemies[i].GetComponent<MonsterEndSpawn>().GetStarted(fmax,ftime);
        }
    }
    void monSpawn2()
    {
         for (int i = 0; i<enemies.Length; i++)
        {
           mot.gameObject.SetActive(true);

            enemies[i].GetComponent<MonsterEndSpawn>().GetStarted(smax, stime);
        }

    }
    void monSpawn3()
    {
        for (int i = 0; i < enemies.Length; i++)
        {

            enemies[i].GetComponent<MonsterEndSpawn>().GetStarted(tmax, ttime);
        }

    }
    // Update is called once per frame
    void Update()
    {
        if (IsBreak())
        {
            Break();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
             monSpawn2();
           
        
       
         
            
            Debug.Log("몬스터 시작");
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
     


            monSpawn3();


            Debug.Log("몬스터 시작");
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
