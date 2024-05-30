using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnManager : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("포탈 설정")]
    [SerializeField]
    private GameObject portal;
    [SerializeField]
    private GameObject portalSpawner;
    [SerializeField]
    private GameObject[] portalSpawnPoints;
    [Header("타이머 설정")]
    [SerializeField]
    private GameObject timer;
    [Header("몬스터 설정")]
    public GameObject[] monsterPrefabs;

    private int spawnnum=0;
    public int cyclopsMax;
    public int goblinMax;
    public int hobgoblinMax;
    public int koboldMax;
    public int TrollMax;

    private GameObject timer2;

    //몬스터 스폰 포인트
    private static SpawnManager instance = null;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

        }
        else
        {

            PhotonNetwork.Destroy(this.gameObject);
        }
    }
    public void TimerDestroy()
    {
        PhotonNetwork.Destroy(timer2);
    }
    public void TimerSpawn()
    {
        if (PhotonNetwork.IsMasterClient)
            timer2 = PhotonNetwork.InstantiateRoomObject(timer.name, transform.position, transform.rotation, 0);
    }
    public GameObject PortalSpawnerSpawn() {
        GameObject portalSpawn =null;
        if (PhotonNetwork.IsMasterClient)
            portalSpawn = PhotonNetwork.InstantiateRoomObject(portalSpawner.name, portalSpawnPoints[spawnnum].transform.position, portalSpawnPoints[spawnnum].transform.rotation, 0);
        if (spawnnum == portalSpawnPoints.Length)
            spawnnum = 0;
        else
            spawnnum++;

        return portalSpawn;
    }
    void Start()
    {
        
        
  
    
    }
    public static SpawnManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    public GameObject getMonster(int type) { 
        return monsterPrefabs[type];
    }
   
    // Update is called once per frame
    void Update()
    {
        
    }

   



}
