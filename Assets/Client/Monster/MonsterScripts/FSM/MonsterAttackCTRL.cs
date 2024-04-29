using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackCTRL : MonoBehaviour
{
    //Monster monster;
    //public Animator Anim { get; private set; }
    public bool isAttack { get; private set; }

    [Header("무기 오브젝트 설정")]
    [SerializeField] EnemyWeapon weapon_right;
    [SerializeField] EnemyWeapon weapon_left;

    private void Awake()
    {
        isAttack= false;
        //monster = GetComponent<Monster>();
        //Anim = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    public void Start()
    {
        
    }
    // called by animator
    public void rightWeaponUse()
    {
        weapon_right.WeaponUse();
    }
    // called by animator
    public void leftWeaponUse()
    {
        weapon_left.WeaponUse();
    }
    // called by animator
    public void WeaponOut()
    {
        weapon_right.WeaponOut();
        if (weapon_left != null)
        {
            weapon_left.WeaponOut();
        }
    }
    // called by animator
    public void IsAttack()
    {
        isAttack = true;
    }

    public void AttackOut()
    {
        isAttack = false;
    }
}
