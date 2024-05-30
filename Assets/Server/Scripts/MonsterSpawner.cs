using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;
public class MonsterSpawner : MonoBehaviour
{
    [SerializeField]
    public int monType; //0: cyclops 1: goblin; 2:hobgoblin 3:kobold 4:Troll
    private int monMax;
    [SerializeField]
    private float detectionRadius = 5f;
    [SerializeField]
    private int createTime;
    private GameObject mon;
    private GameObject monster;
    private int monNum;
    int flow=0;
    int mode = 0;
    
    MonsterCounter monsterCounter;
    // Start is called before the first frame update
    void OnEnable()
    {     
        if (PhotonNetwork.IsMasterClient&&flow==1)
            Spawn();

    }
    void Start()
    {
        mon = SpawnManager.Instance.getMonster(monType);
        //monsterCounter = this.transform.parent.GetComponent<MonsterCounter>();
        //monMax = monsterCounter.monMax;
       // monNum = monsterCounter.monMax;
       if(PhotonNetwork.IsMasterClient)
            Spawn();
        // StartCoroutine("SpawnMon");
        Debug.Log(mon+"몬스터");
        flow = 1;
    }
    IEnumerator SpawnMon()
    {
        while (!GameManager.Instance.isGameover&&GameManager.Instance.IsMaster())
        {
           
            monNum = monsterCounter.controlMonNum();
            if (monNum < monMax)
            {
                Debug.Log("소환");
                 PhotonNetwork.InstantiateRoomObject(mon.name, transform.position, transform.rotation, 0);
                //Instantiate(mon, transform.position, transform.rotation);
                yield return new WaitForSeconds(createTime + Random.Range(0, 5));
            }
            else
            {
                Debug.Log("실패");
                //yield return new WaitForSeconds(createTime);
                yield return new WaitForSeconds(createTime + Random.Range(0, 5)); // 몬스터 죽고 1분 후 생성
            }
        }
    }
   
    void Spawn()
    {
        Debug.Log("포톤 네트워크 확인 " + PhotonNetwork.IsMasterClient);
        Debug.Log("포톤 네트워크 확인 " + GameManager.Instance.IsMaster());
        monster = PhotonNetwork.Instantiate(mon.name, transform.position, transform.rotation, 0);
        mode = 1;
    }
    void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        if (monster == null&&mode ==1)
        {
            Invoke("Spawn", createTime);
            mode = 0;
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




}
