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
    [SerializeField] int ftime = 3;
    [SerializeField] int smax = 3;
    [SerializeField] int stime = 2;
    [SerializeField] int tmax = 5;
    [SerializeField] int ttime = 1;
    [SerializeField] int waittime =5;
    public GameObject mot; 
    int mod = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public bool getDefense()
    {
        return true;
    }
    void OnEnable()
    {
        mot.gameObject.SetActive(false);
        Invoke("monSpawn", waittime+10);
        SpawnManager.Instance.TimerDestroy();
        GameManager.Instance.setTime = waittime + 10;
        SpawnManager.Instance.TimerSpawn();


    }
    void monSpawn()
    {
        Debug.Log("1라");
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].gameObject.SetActive(true);
            enemies[i].GetComponent<MonsterEndSpawn>().GetStarted(fmax, ftime);
        }
        Invoke("monSpawn2", waittime + 10);
        SpawnManager.Instance.TimerDestroy();
        GameManager.Instance.setTime = waittime + 10;
        SpawnManager.Instance.TimerSpawn();
    }
    void monSpawn2()
    {
        Debug.Log("2라");
        mot.gameObject.SetActive(true);
        for (int i = 0; i<enemies.Length; i++)
        {
          

            enemies[i].GetComponent<MonsterEndSpawn>().GetStarted(smax, stime);
        }
        Invoke("monSpawn3", waittime + 15);
        SpawnManager.Instance.TimerDestroy();
        GameManager.Instance.setTime = waittime + 15;
        SpawnManager.Instance.TimerSpawn();
    }
    void monSpawn3()
    {
        Debug.Log("3라");
        for (int i = 0; i < enemies.Length; i++)
        {

            enemies[i].GetComponent<MonsterEndSpawn>().GetStarted(tmax, ttime);
        }
        Invoke("GameFinish", waittime + 20);
        SpawnManager.Instance.TimerDestroy();
        GameManager.Instance.setTime = waittime + 20;
        SpawnManager.Instance.TimerSpawn();
    }
    void GameFinish()
    {

        GameManager.Instance.GameFinish();
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
