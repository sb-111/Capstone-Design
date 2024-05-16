using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashEffectCollider : MonoBehaviour
{
    PrefabCreator prefabCreator;
    private HashSet<GameObject> hitEnemies = new HashSet<GameObject>();
    
    //�浹 ȿ�� ����
    Vector3 hitPos;
    public GameObject hitEffectPrefab;

    // Start is called before the first frame update
    void Start()
    {
        prefabCreator = GetComponentInParent<PrefabCreator>();
        hitEnemies = prefabCreator.weapon.GethitEnemeies();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "MonsterEnemy" || other.tag == "Enemy" || other.tag == "Player")
        {
            GameObject enemy = other.gameObject;

            if (other.tag == "MonsterEnemy")
            {
                Monster enemyDamage = enemy.GetComponentInParent<Monster>();
                if (!hitEnemies.Contains(enemy)) // �̹� ������ ���� �ƴ϶��
                {
                    //�浹 ������ ����
                    hitPos = other.ClosestPointOnBounds(transform.position);
                    GameObject hitEffect = Instantiate(hitEffectPrefab, hitPos, this.transform.rotation);
                    Destroy(hitEffect,0.5f);

                   
                    prefabCreator.weapon.AddToHitEnemeies(enemy); // �� ���� ������ �� ��Ͽ� �߰� //enemyDamage.curHP -= damage;//++ ���⿡ enemy���� ������ �����ϴ� ���� �߰� //if (hitEnemies.Contains(enemy))    {Debug.Log("�߰���");  }
                    enemyDamage.TakeDamage((prefabCreator.result_damage));
                  
                    if (prefabCreator.weapon.isHeavyAttack) //�������� ��� �ǰ� ���� �ִϸ��̼� ó��
                    {
                        enemyDamage.HitResponse();
                    }
                }
            }
            else if (other.tag == "Player")
            {

                if (enemy == prefabCreator.weapon.player.gameObject) { return; }
                CombatStatusManager enemyDamage = enemy.GetComponent<CombatStatusManager>();
                if (!hitEnemies.Contains(enemy)) // �̹� ������ ���� �ƴ϶��
                {
                    //�浹 ������ ����
                    hitPos = other.ClosestPointOnBounds(transform.position);
                    GameObject hitEffect = Instantiate(hitEffectPrefab, hitPos, this.transform.rotation);
                    Destroy(hitEffect, 0.5f);

                    prefabCreator.weapon.AddToHitEnemeies(enemy); // �� ���� ������ �� ��Ͽ� �߰� //enemyDamage.curHP -= damage;//++ ���⿡ enemy���� ������ �����ϴ� ���� �߰� //if (hitEnemies.Contains(enemy))    {Debug.Log("�߰���");  }
                    enemyDamage.TakeDamage((prefabCreator.result_damage));
                    if (prefabCreator.weapon.isHeavyAttack) //���⼭ HeavyAttack false�� �ϸ�, ���� �ǰ� ������ ���� �ȵ�
                    {
                        enemyDamage.HitResponse();
                    }
                }
            }

            else if (other.tag == "Enemy")
            {

                Hit_Test_v2 enemyDamage = enemy.GetComponent<Hit_Test_v2>();
                if (!hitEnemies.Contains(enemy)) // �̹� ������ ���� �ƴ϶��
                {
                    hitEnemies.Add(enemy); // �� ���� ������ �� ��Ͽ� �߰� //enemyDamage.curHP -= damage;//++ ���⿡ enemy���� ������ �����ϴ� ���� �߰� //if (hitEnemies.Contains(enemy))    {Debug.Log("�߰���");  }
                    enemyDamage.TakeDamage((prefabCreator.result_damage));
                    GameObject hiteffectInstance = Instantiate(hitEffectPrefab, other.ClosestPointOnBounds(transform.position), Quaternion.identity);
                    Destroy(hiteffectInstance, 0.5f);
                }
            }
            else return;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
