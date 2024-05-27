using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MonsterEndSpawn : MonoBehaviourPun
{
    // Start is called before the first frame update
    public GameObject mon;
    public GameObject boss;
    int max;
    int num=0;
    int time;
    void OnEnable()
    {
        
        
    }

    public void GetStarted(int x,int y)
    {
        max = x;
        time = y;
        StartCoroutine("SpawnMon");
    }
    IEnumerator SpawnMon()
    {
        while (max>=num&& PhotonNetwork.IsMasterClient)
          {
            Debug.Log(num + "몬스터 생성 체크");
           // Instantiate(mon, transform.position + new Vector3(2, 0, 4), transform.rotation);
            PhotonNetwork.InstantiateRoomObject(mon.name, transform.position+new Vector3(4,0,0), transform.rotation);
            PhotonNetwork.InstantiateRoomObject(boss.name, transform.position + new Vector3(0, 0, 6), transform.rotation);
            PhotonNetwork.InstantiateRoomObject(mon.name, transform.position + new Vector3(-4, 0, 0), transform.rotation);
            //Instantiate(mon, transform.position + new Vector3(-2, 0, -4), transform.rotation);
            num++;
            yield return new WaitForSeconds(time);
         
         }
    }
    // Update is called once per frame
    void Update()
    {
       
    }
}
