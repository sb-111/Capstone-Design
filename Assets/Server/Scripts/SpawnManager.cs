using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnManager : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject monster;
    public GameObject timer;
    void Start()
    {
        PhotonNetwork.InstantiateRoomObject(monster.name, transform.position, transform.rotation, 0);
        PhotonNetwork.InstantiateRoomObject(timer.name, transform.position, transform.rotation, 0);
        //지금 연결된 오브젝트 위치에서 생성되게 설정되어있음 실제로 제작할 때는 값을 넣는 방식이나
        //public GameObject spawnpoint 위치 지정으로 바꾸는 게 나을듯...
        //instantiate와 instatiateRoomObject와의 차이 : 전자는 서버에서 나가면 방 파괴 후자는 남아있음 
        //마스터만 생성할 수 있음 이거 이용해서 타이머 수정... 하면 좋고 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
