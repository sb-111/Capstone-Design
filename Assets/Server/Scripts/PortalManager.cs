using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PortalManager : MonoBehaviourPun
{
    [SerializeField] int currentHP = 100;
    [SerializeField] GameObject[] enemies;
    [SerializeField] int[] max;
    [SerializeField] int[] time;
    public int gameTime;
    [SerializeField] int smax = 3;
    [SerializeField] int stime = 2;
    [SerializeField] int tmax = 5;
    [SerializeField] int ttime = 1;
    [SerializeField] int waittime =5;
    public GameObject mot; 
    bool isGame = false;
    public GameObject fportal;
    public GameObject sportal;
    int mode = 0;
    int phase = 1;
    int monnum = 0;
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
        fportal.gameObject.SetActive(false);
        sportal.gameObject.SetActive(false);
        isGame = true;
        phase = 0;
        StartCoroutine("Defense");
     
    }
    IEnumerator Defense()
    {
        while (true)
        {
            Debug.Log(phase + "코루틴");
            if (phase == 2)
            {
                GameFinish();
                yield break;
            }
            else if (!isGame)
            {
                Debug.Log(phase + "라");
                SpawnManager.Instance.TimerDestroy();
                GameManager.Instance.setTime = gameTime;
                SpawnManager.Instance.TimerSpawn();
                for (int i = 0; i < enemies.Length; i++)
                {
                    enemies[i].gameObject.SetActive(true);
                    enemies[i].GetComponent<MonsterEndSpawn>().GetStarted(max[phase], time[phase]);
                }
                yield return new WaitForSeconds(gameTime);
                phase++;
                monDelete();
            }
            else if (isGame)
            {
                Debug.Log(phase + "휴식");
                SpawnManager.Instance.TimerDestroy();
                GameManager.Instance.setTime = waittime;
                SpawnManager.Instance.TimerSpawn();
                yield return new WaitForSeconds(waittime);
            }
            isGame = !isGame;
        }
    }

    void monSpawn()
    {
        Debug.Log("1라");
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].gameObject.SetActive(true);
           // enemies[i].GetComponent<MonsterEndSpawn>().GetStarted(fmax, ftime);
        }
        phase = 1;
        isGame = true;
        Invoke("Wait", waittime);
        SpawnManager.Instance.TimerDestroy();
        GameManager.Instance.setTime = waittime;
        SpawnManager.Instance.TimerSpawn();
    }
    void monSpawn2()
    {
        monDelete();
        Debug.Log("2라");
        mot.gameObject.SetActive(true);
        fportal.gameObject.SetActive(true);
        for (int i = 0; i<enemies.Length; i++)
        {
          

            enemies[i].GetComponent<MonsterEndSpawn>().GetStarted(smax, stime);
        }
        phase = 2;
        isGame = true;
        Invoke("Wait", waittime);
        SpawnManager.Instance.TimerDestroy();
        GameManager.Instance.setTime = waittime;
        SpawnManager.Instance.TimerSpawn();
    }
    void monSpawn3()
    {
        monDelete();
        sportal.gameObject.SetActive(true);
        Debug.Log("3라");
        for (int i = 0; i < enemies.Length; i++)
        {

            enemies[i].GetComponent<MonsterEndSpawn>().GetStarted(tmax, ttime);
        }
        phase = 3;
        isGame = true;
    }
    void monDelete()
    {
        GameObject[] mons = GameObject.FindGameObjectsWithTag("MonsterEnemy");
        foreach (GameObject mon in mons)
        {

            PhotonNetwork.Destroy(mon);
        }
    }
    void GameFinish()
    {
        monDelete();
        GameManager.Instance.GameFinish();
    }
    int MonCheck()
    {
        monnum = 0;
         GameObject[] mons = GameObject.FindGameObjectsWithTag("MonsterEnemy");
            foreach (GameObject mon in mons)
            {
            monnum++;
            }
        return monnum;
    }
    // Update is called once per frame
    void Update()
    {
     
        //if (isGame&& MonCheck()==0)
        //{
        //    isGame = false;
        //    switch (line)
        //    {
        //        case 1:
        //            Invoke("monSpawn2", waittime);
        //            SpawnManager.Instance.TimerDestroy();
        //            GameManager.Instance.setTime = waittime;
        //            SpawnManager.Instance.TimerSpawn();
        //            break;
        //        case 2:
        //            Invoke("monSpawn3", waittime);
        //            SpawnManager.Instance.TimerDestroy();
        //            GameManager.Instance.setTime = waittime;
        //            SpawnManager.Instance.TimerSpawn();
        //            break;
        //        case 3:
        //            GameFinish();
        //            break;
        //    }


        //}
        if (IsBreak())
        {
            Break();
        }
  
    }

    void OnDestroy()
    {
        SpawnManager.Instance.PortalSpawnerSpawn();
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
