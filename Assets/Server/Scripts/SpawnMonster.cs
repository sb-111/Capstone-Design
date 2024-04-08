using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnMonster : MonoBehaviour
{
    public Transform[] monsterSpawnPoints;
    public GameObject crocodile;

    // Start is called before the first frame update
    void Start()
    {
        SpawnCrocodile();
      //  SpawnCrocodile();
    }

    void SpawnCrocodile()
    {
        int number = Random.Range(0, monsterSpawnPoints.Length);
        PhotonNetwork.InstantiateRoomObject(crocodile.name, monsterSpawnPoints[number].transform.position, transform.rotation, 0);

    }

    // Update is called once per frame
    void Update()
    {
        //특정 조건 생성
    }
}
