using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicTower : MonoBehaviour
{
    [SerializeField]
    private float detectionRadius = 5f; // 몬스터 감지 범위
    private Vector3 lastMonsterPosition; // 마지막으로 발견된 몬스터의 위치
    private bool hasMonsterPosition = false; // 몬스터의 위치를 가지고 있는지 여부
    [SerializeField]
    private float cooldown = 3f; // 마법 발사 쿨타임
    private float nextFireTime = 0f; // 다음 마법 발사 시간
    private bool isCoolingDown = false; // 쿨타임 중인지 여부

    void Start()
    {

    }



    void Update()
    {
        if (!isCoolingDown && !hasMonsterPosition)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position - new Vector3(0, 4, 0), detectionRadius);

            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Player"))
                {


                    lastMonsterPosition = collider.transform.position + new Vector3(0, 1, 0);
                    transform.LookAt(lastMonsterPosition);
                    hasMonsterPosition = true;
                    StartCooldown(); //쿨타임
                    break;
                }
            }
        }

        if (Time.time >= nextFireTime && hasMonsterPosition)
        {
            ShootMagic();
            hasMonsterPosition = false; //위치 초기화
        }
    }

    void ShootMagic()
    {



        var bullet = ObjectPool.GetObject(transform);



        nextFireTime = Time.time + cooldown;
        return;


    }

    void StartCooldown()
    {
        isCoolingDown = true;
        Invoke("EndCooldown", cooldown);
    }

    void EndCooldown()
    {

        isCoolingDown = false;
    }

    void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position - new Vector3(0, 4, 0), detectionRadius);
    }
}
