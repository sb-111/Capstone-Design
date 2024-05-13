using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnMonster : MonoBehaviour
{
    public Transform[] cyclopsSpawnPoints;
    public Transform[] goblinSpawnPoints;
    public Transform[] hobgoblinSpawnPoints;
    public Transform[] koboldSpawnPoints;
    public Transform[] TrollSpawnPoints;
    public GameObject cyclops;
    public GameObject goblin;
    public GameObject hobgoblin;
    public GameObject kobold;
    public GameObject Troll;
    // Start is called before the first frame update
    void Start()
    {
        SpawnCyclops();
      //  SpawnCrocodile();
    }

    void SpawnCyclops()
    {
        int number= cyclopsSpawnPoints.Length;
        for (int i = 0; i < number; i++) {
            PhotonNetwork.InstantiateRoomObject(cyclops.name, cyclopsSpawnPoints[number].transform.position, transform.rotation, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //특정 조건 생성
    }
}
