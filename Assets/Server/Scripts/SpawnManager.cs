using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnManager : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject monster;
    public GameObject timer;
    public GameObject portal;
    public GameObject portalSpawner;
    public GameObject portalSpawnPoint;
    //포탈 스폰 포인트
    public GameObject monsterSpawnPoint;
    //몬스터 스폰 포인트
    private static SpawnManager instance = null;
    void Start()
    {
        PhotonNetwork.InstantiateRoomObject(timer.name, transform.position, transform.rotation, 0);
        PhotonNetwork.InstantiateRoomObject(portalSpawner.name, portalSpawnPoint.transform.position, transform.rotation, 0);
        //지금 연결된 오브젝트 위치에서 생성되게 설정되어있음 실제로 제작할 때는 값을 넣는 방식이나
        //public GameObject spawnpoint 위치 지정으로 바꾸는 게 나을듯...
        //instantiate와 instatiateRoomObject와의 차이 : 전자는 서버에서 나가면 방 파괴 후자는 남아있음 
        //마스터만 생성할 수 있음 이거 이용해서 타이머 수정... 하면 좋고 
        if (instance == null)
        {
            instance = this;
     
        }
        else
        {
            
            Destroy(this.gameObject);
        }
  
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void portalSpawn()
    {
        PhotonNetwork.InstantiateRoomObject(portal.name, portalSpawnPoint.transform.position, transform.rotation, 0);
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



}
