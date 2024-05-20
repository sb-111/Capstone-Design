using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;
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
    int flow=0;
    // Start is called before the first frame update
    void Start()
    {
        mon = SpawnManager.Instance.getMonster(monType);
        monNum = 0;
        StartCoroutine("SpawnMon");
        Debug.Log(mon+"몬스터");
    }
    IEnumerator SpawnMon()
    {
        while (!GameManager.Instance.isGameover)
        {

            if (monNum < monMax)
            {
               
                randspawn();
                yield return new WaitForSeconds(createTime);
            }
            else
            {
                yield return null;
            }
        }
    }
    void randspawn()
    {
        Debug.Log("실행됨" + mon);
        float randomAngle = Random.Range(0f, Mathf.PI * 2f);
        float randX = Mathf.Cos(randomAngle) * detectionRadius;
        float randZ = Mathf.Sin(randomAngle) * detectionRadius;
        Vector3 randomPosition = transform.position + new Vector3(randX, 0f, randZ);
        NavMeshHit hit;
       // if (NavMesh.SamplePosition(randomPosition, out hit, 0.1f, NavMesh.AllAreas))
       // {
            PhotonNetwork.InstantiateRoomObject(mon.name, randomPosition, transform.rotation, 0);
     //   }
       

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
