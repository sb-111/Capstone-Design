using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    public enum WeaponType { Melee, Range };
    public WeaponType type;
    public int weapon_damage = 30; //무기별 공격력
    //public float weapon_rate; // 무기별 공격 속도
    public BoxCollider meleeArea;   //무기의 공격 판정 범위
    //public TrailRenderer trailEffect; //공격시 생성 이펙트
    private HashSet<GameObject> hitEnemies = new HashSet<GameObject>();
    private Animator Anim;
    private Monster monster;
    private void Start()
    {
        
    }
    private void Awake()
    {
        Anim= GetComponentInParent<Animator>();
        monster= GetComponentInParent<Monster>();
    }
    
    public void WeaponUse()
    {
        hitEnemies.Clear();
        meleeArea.enabled = true;
    }

    public void WeaponOut()
    {
        meleeArea.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameObject player = other.gameObject;
            CombatStatusManager combatStatus = player.GetComponent<CombatStatusManager>();
            if (!hitEnemies.Contains(player)) // 이미 공격한 적이 아니라면
            {
                hitEnemies.Add(player); 
                combatStatus.TakeDamage((20 + weapon_damage));
            }
        }

        /*  
        if (other.tag == "Melee")
        {
            Anim.SetTrigger("doParrying");
        }
        */ 
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
