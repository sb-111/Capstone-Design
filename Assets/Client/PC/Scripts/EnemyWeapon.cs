using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    public enum WeaponType { Melee, Range };
    public WeaponType type;
    public int weapon_damage=30; //무기별 공격력
    public float weapon_rate; // 무기별 공격 속도
    public BoxCollider meleeArea;   //무기의 공격 판정 범위
    //public TrailRenderer trailEffect; //공격시 생성 이펙트
    private HashSet<GameObject> hitEnemies = new HashSet<GameObject>();

    private void Start()
    {
        
    }
    private void Awake()
    {
        
    }
    public void Use(float attackEndTime)
    {
        if (type == WeaponType.Melee)
        {
            StopCoroutine(Weapon_Activation(attackEndTime));
            hitEnemies.Clear();                         //HashSet 초기화, 공격이 새롭게 시작될 때 마다 초기화.
            //Debug.Log("HashSet 클리어");

            StartCoroutine(Weapon_Activation(attackEndTime));
        }

        if (type == WeaponType.Range)
        {
            StopCoroutine(Weapon_Activation(attackEndTime));
            hitEnemies.Clear();                         //HashSet 초기화, 공격이 새롭게 시작될 때 마다 초기화.

            //Debug.Log("HashSet 클리어");

            StartCoroutine(Weapon_Activation(attackEndTime));
        }
    }

    IEnumerator Weapon_Activation(float attackEndTime)
    {
        meleeArea.enabled = true;
        //trailEffect.enabled = true;
        yield return new WaitForSeconds(attackEndTime);
        meleeArea.enabled = false;
        yield return new WaitForSeconds(0.2f);
        //trailEffect.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameObject enemy = other.gameObject;
            Player enemyDamage = enemy.GetComponent<Player>();
            if (!hitEnemies.Contains(enemy)) // 이미 공격한 적이 아니라면
            {
                hitEnemies.Add(enemy); 
                enemyDamage.TakeDamage((20 + weapon_damage), transform.position);
            }
        }
      
    }
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("확인");
        if (other.collider.CompareTag( "Build"))
        {
            GameObject build = other.gameObject;
            Barricade buildDamage = build.GetComponent<Barricade>();
            if (!hitEnemies.Contains(build)) // 이미 공격한 적이 아니라면
            {
                hitEnemies.Add(build);
                buildDamage.TakeDamage((20 + weapon_damage), transform.position);
            }
        }
    }


}
