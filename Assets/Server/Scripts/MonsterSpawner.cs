using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField]
    public int monType; //0: cyclops 1: goblin; 2:hobgoblin 3:kobold 4:Troll
    [SerializeField]
    private int monMax;
    [SerializeField]
    private float detectionRadius = 5f;
    [SerializeField]
    private int createTime;
    private GameObject mon;
    private int monNum;
    // Start is called before the first frame update
    void Start()
    {
        mon = SpawnManager.Instance.getMonster(monType);
        monNum = 0;
        StartCoroutine("SpawnMon");
    }
    IEnumerator SpawnMon()
    {
        while (!GameManager.Instance.isGameover)
        {

            if (monNum < monMax)
            {
                PhotonNetwork.InstantiateRoomObject(mon.name, transform.position, transform.rotation, 0);
                yield return new WaitForSeconds(createTime);
            }
            else
            {
                yield return null;
            }
        }
    }

    void controlMonNum()
    {
        int a=0;
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("MonsterEnemy"))
            {
                a++; // 대상 태그를 가진 물체이면 개수 증가
            }
        }
        Debug.Log(a+"몬스터 감지 개수"+monNum);
        monNum = a;

       
       
    }
    // Update is called once per frame
    void Update()
    {
        controlMonNum();

    }
    void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position , detectionRadius);
    }
}
