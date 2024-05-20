using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssassinStrongEffectTransfrom : MonoBehaviour
{
    
    private HashSet<GameObject> hitEnemies = new HashSet<GameObject>();
    private PrefabCreator prefabCreator;
    private BoxCollider boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        prefabCreator = GetComponentInParent<PrefabCreator>();
        transform.rotation=prefabCreator.weapon.player.transform.rotation;
        boxCollider= GetComponent<BoxCollider>();
    }
    
    private void Update()
    {
        transform.position += transform.forward*0.3f;
        transform.position += transform.up * -0.1f;
    }

    // Update is called once per frame
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
                    if (prefabCreator == null) Debug.Log("프리팹크리에이터 null");
                    enemyDamage.TakeDamage((prefabCreator.result_damage));
                }
            }
            else return;
        }
    }
}
