using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorStrongEffectTrasnform : MonoBehaviour
{

    new BoxCollider collider;
    float targetCenterZ = 14f;
    float duration = 1f;

    Vector3 initialCenter;
    float startTime;
    // Start is called before the first frame update

    private HashSet<GameObject> hitEnemies = new HashSet<GameObject>();
    private PrefabCreator prefabCreator;
    void Start()
    {
        collider = GetComponent<BoxCollider>();
        prefabCreator = GetComponentInParent<PrefabCreator>();
        initialCenter = collider.center;
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float timeElapsed = Time.time - startTime;
        float progress = timeElapsed / duration;

        if (progress < 1.0f)
        {
            Vector3 newCenter = collider.center;
            newCenter.z = Mathf.Lerp(initialCenter.z, targetCenterZ, progress);
            collider.center = newCenter;
        }
        else if (collider.center.z != targetCenterZ)
        {
            // 목표 위치에 정확히 맞추기
            Vector3 finalCenter = collider.center;
            finalCenter.z = targetCenterZ;
            collider.center = finalCenter;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "MonsterEnemy" || other.tag == "Enemy" || other.tag == "Player")
        {
            GameObject enemy = other.gameObject;

            if (other.tag == "MonsterEnemy")
            {
                Monster enemyDamage = enemy.GetComponentInParent<Monster>();
                if (!hitEnemies.Contains(enemy)) // 이미 공격한 적이 아니라면
                {
                    hitEnemies.Add(enemy); // 이 적을 공격한 적 목록에 추가 //enemyDamage.curHP -= damage;//++ 여기에 enemy에게 데미지 적용하는 라인 추가 //if (hitEnemies.Contains(enemy))    {Debug.Log("추가됨");  }
                    enemyDamage.TakeDamage((prefabCreator.result_damage));
                    enemyDamage.HitResponse();
                }
            }
            else if (other.tag == "Player")
            {
                if (enemy == prefabCreator.weapon.player.gameObject) { return; }
                CombatStatusManager enemyDamage = enemy.GetComponent<CombatStatusManager>();
                if (!hitEnemies.Contains(enemy)) // 이미 공격한 적이 아니라면
                {
                    hitEnemies.Add(enemy); // 이 적을 공격한 적 목록에 추가 //enemyDamage.curHP -= damage;//++ 여기에 enemy에게 데미지 적용하는 라인 추가 //if (hitEnemies.Contains(enemy))    {Debug.Log("추가됨");  }
                    enemyDamage.TakeDamage((prefabCreator.result_damage));
                    enemyDamage.HitResponse();
                }
            }

            else if (other.tag == "Enemy")
            {

                Hit_Test_v2 enemyDamage = enemy.GetComponent<Hit_Test_v2>();
                if (!hitEnemies.Contains(enemy)) // 이미 공격한 적이 아니라면
                {
                    hitEnemies.Add(enemy); // 이 적을 공격한 적 목록에 추가 //enemyDamage.curHP -= damage;//++ 여기에 enemy에게 데미지 적용하는 라인 추가 //if (hitEnemies.Contains(enemy))    {Debug.Log("추가됨");  }
                    if(prefabCreator==null)Debug.Log("프리팹크리에이터 null");
                    enemyDamage.TakeDamage((prefabCreator.result_damage));
                }
            }
            else return;
        }
    }

}
