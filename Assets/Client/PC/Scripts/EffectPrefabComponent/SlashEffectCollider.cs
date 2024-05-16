using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashEffectCollider : MonoBehaviour
{
    PrefabCreator prefabCreator;
    private HashSet<GameObject> hitEnemies = new HashSet<GameObject>();
    
    //충돌 효과 관련
    Vector3 hitPos;
    public GameObject hitEffectPrefab;
    WeaponSoundEffect soundEffect;

    // Start is called before the first frame update
    void Start()
    {
        prefabCreator = GetComponentInParent<PrefabCreator>();
        hitEnemies = prefabCreator.weapon.GethitEnemeies();
        soundEffect=GetComponent<WeaponSoundEffect>();
        Invoke("PlaySound", 0.05f);
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
                    //충돌 프리팹 생성
                    hitPos = other.ClosestPointOnBounds(transform.position);
                    GameObject hitEffect = Instantiate(hitEffectPrefab, hitPos, this.transform.rotation);
                     soundEffect.PlayWeaponSound("Monster");
                    Destroy(hitEffect,0.5f);

                   
                    prefabCreator.weapon.AddToHitEnemeies(enemy); // 이 적을 공격한 적 목록에 추가 //enemyDamage.curHP -= damage;//++ 여기에 enemy에게 데미지 적용하는 라인 추가 //if (hitEnemies.Contains(enemy))    {Debug.Log("추가됨");  }
                    enemyDamage.TakeDamage((prefabCreator.result_damage));
                  
                    if (prefabCreator.weapon.isHeavyAttack) //강공격일 경우 피격 반응 애니메이션 처리
                    {
                        enemyDamage.HitResponse();
                    }
                }
            }
            else if (other.tag == "Player")
            {

                if (enemy == prefabCreator.weapon.player.gameObject) { return; }
                CombatStatusManager enemyDamage = enemy.GetComponent<CombatStatusManager>();
                if (!hitEnemies.Contains(enemy)) // 이미 공격한 적이 아니라면
                {
                    //충돌 프리팹 생성
                    hitPos = other.ClosestPointOnBounds(transform.position);
                    GameObject hitEffect = Instantiate(hitEffectPrefab, hitPos, this.transform.rotation);
                    soundEffect.PlayWeaponSound("Monster");
                    Destroy(hitEffect, 0.5f);

                    prefabCreator.weapon.AddToHitEnemeies(enemy); // 이 적을 공격한 적 목록에 추가 //enemyDamage.curHP -= damage;//++ 여기에 enemy에게 데미지 적용하는 라인 추가 //if (hitEnemies.Contains(enemy))    {Debug.Log("추가됨");  }
                    enemyDamage.TakeDamage((prefabCreator.result_damage));
                    if (prefabCreator.weapon.isHeavyAttack) //여기서 HeavyAttack false로 하면, 이후 피격 대상들이 적용 안됨
                    {
                        enemyDamage.HitResponse();
                    }
                }
            }

            else if (other.tag == "Enemy")
            {

                Hit_Test_v2 enemyDamage = enemy.GetComponent<Hit_Test_v2>();
                if (!hitEnemies.Contains(enemy)) // 이미 공격한 적이 아니라면
                {
                    hitEnemies.Add(enemy); // 이 적을 공격한 적 목록에 추가 //enemyDamage.curHP -= damage;//++ 여기에 enemy에게 데미지 적용하는 라인 추가 //if (hitEnemies.Contains(enemy))    {Debug.Log("추가됨");  }
                    enemyDamage.TakeDamage((prefabCreator.result_damage));
                    GameObject hiteffectInstance = Instantiate(hitEffectPrefab, other.ClosestPointOnBounds(transform.position), Quaternion.identity);
                    soundEffect.PlayWeaponSound("Monster");
                    Destroy(hiteffectInstance, 0.5f);
                }
            }
            else return;
        }
    }
    // Update is called once per frame
    void PlaySound()
    {
        soundEffect.PlayWeaponSound("Weapon");
    }
}
