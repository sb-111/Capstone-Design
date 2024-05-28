using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    public enum WeaponType { Melee, Range };
    public WeaponType type;
    public int weapon_damage = 30; //무기별 공격력
    public float knockbackPower = 10f;
    private bool isStrongAttack = false;

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
        Anim = GetComponentInParent<Animator>();
        monster = GetComponentInParent<Monster>();
    }
    
    public void WeaponUse()
    {
        hitEnemies.Clear();
        meleeArea.enabled = true; // Collider 활성화
    }

    public void WeaponOut()
    {
        meleeArea.enabled = false; // Collider 비활성화
        isStrongAttack = false;
        
    }
    public void SetStrongAttack()
    {
        isStrongAttack = true;  
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("닿음"+other.tag);
        if (other.tag == "Player")
        {
            
            GameObject player = other.gameObject;
            CombatStatusManager combatStatus = player.GetComponent<CombatStatusManager>();
            if (!hitEnemies.Contains(player)) // 이미 공격한 적이 아니라면
            {
                hitEnemies.Add(player);  // 적 HashSet에 추가
                combatStatus.TakeDamage((20 + weapon_damage));
                /*if(isStrongAttack) // 강공격인 경우
                {
                    Vector3 knockbackDir = (player.transform.position - transform.position); // 넉백 방향
                    knockbackDir.y = 0;
                    knockbackDir = knockbackDir.normalized;
                    combatStatus.TakeKnockback(knockbackDir);
                }*/
            }
        }


        if (other.tag == "Melee")
        {
            Weapon weapon = other.GetComponent<Weapon>();
            if (weapon.parryingAttack)
            {
                Anim.SetTrigger("Parried");
                StunEffect stun = monster.gameObject.AddComponent<StunEffect>();
                stun.Duration = 5.0f; //스턴 시간
                stun.OnStart();

                Debug.Log("패링당했습니다.");
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
