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
    private GameObject portalSpawnPoint;
    [Header("타이머 설정")]
    [SerializeField]
    private GameObject timer;
    [Header("몬스터 설정")]
    public GameObject[] monsterPrefabs;
    public GameObject goblin;
    public GameObject hobgoblin;
    public GameObject kobold;
    public GameObject Troll;
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
        timer2= PhotonNetwork.InstantiateRoomObject(timer.name, transform.position, transform.rotation, 0);

    }
    public void PortalSpawnerSpawn() {
        PhotonNetwork.InstantiateRoomObject(portalSpawner.name, portalSpawnPoint.transform.position, portalSpawnPoint.transform.rotation, 0);
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
    public void portalSpawn()
    {
        PhotonNetwork.InstantiateRoomObject(portal.name, portalSpawnPoint.transform.position, portalSpawnPoint.transform.rotation, 0);
    }
   



}
