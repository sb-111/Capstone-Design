using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class PortalManager : MonoBehaviourPun
{
    [SerializeField] int currentHP = 100;
    [SerializeField] private Slider hpBar;
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
    public float portalTime = 225f;
    private PhotonView PV;
   // public AudioClip[] phaseBGMs; 
    //private AudioClip nowBGM;
   // private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        //audioSource = GetComponent<AudioSource>();
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
        if (GameManager.Instance.IsMaster())
            StartCoroutine("Defense");
        StartCoroutine("GrowOverTime");
        SetHealth(currentHP);
    }
    IEnumerator GrowOverTime()
    {
        float progressTime = 0f;
        mot.transform.localScale = new Vector3(1, 1, 1);
        mot.gameObject.SetActive(true);
        while (progressTime < portalTime)
        {

            progressTime += Time.deltaTime;

        
            float progress = Mathf.Clamp01(progressTime / portalTime);

           
            mot.transform.localScale = Vector3.Lerp(new Vector3(0,0,0), new Vector3(1,1,1), progress);

            yield return null;
        }
    
        mot.transform.localScale = new Vector3(1, 1, 1);
    }
    IEnumerator Defense()
    {
        while (true)
        {
            Debug.Log(phase + "코루틴");
            if (phase == 3)
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


    void monDelete()
    {
        GameObject[] mons = GameObject.FindGameObjectsWithTag("MonsterEnemy");
        foreach (GameObject mon in mons)
        {
            if(mon!=null||mon.GetComponent<PhotonView>()!=null)
                GameManager.Instance.ObjDelete(mon);
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
    private void SetHealth(int x)
    {
        hpBar.maxValue = x;
        hpBar.value = x;
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
        //SpawnManager.Instance.PortalSpawnerSpawn();
    }


    void OnTriggerEnter(Collider coll)
    {
        Debug.Log("충돌"+coll.tag);
        if (coll.tag == "Melee"||coll.tag == "EnemyWeapon")
        {
            PV.RPC("TakeDamage", RpcTarget.All);
        }
    }
    [PunRPC]
    public void TakeDamage()
    {
        currentHP -= 10;
        Debug.LogWarning("무기충돌");
        hpBar.value = currentHP;
    }

    private bool IsBreak()
    {
        return currentHP <= 0;
    }
    void OnCollisionEnter(Collision col)
    {
        Debug.Log("충돌");
        if (col.gameObject.CompareTag("EnemyWeapon"))
        {
            Debug.Log("적충돌");
        }
    }

 
    private void Break()
    {
        // 파괴
        SpawnManager.Instance.TimerDestroy();
        monDelete();
        GameManager.Instance.Defencefail();
     
        //PhotonNetwork.Destroy(this.gameObject);

    }
}
