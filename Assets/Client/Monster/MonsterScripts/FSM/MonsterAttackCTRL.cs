using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackCTRL : MonoBehaviour
{
    //public bool isAttack { get; private set; }
    [Header("무기 오브젝트 설정")]
    [SerializeField] EnemyWeapon weapon_right;
    [SerializeField] EnemyWeapon weapon_left;

    private void Awake()
    {
        //isAttack= false;
    }
    // Start is called before the first frame update
    public void Start()
    {
        
    }
    // called by animation clip
    public void rightWeaponUse()
    {
        if (weapon_right != null)
            weapon_right.WeaponUse();
    }
    // called by animation clip
    public void leftWeaponUse()
    {
        if (weapon_left != null)
            weapon_left.WeaponUse();
    }
    // called by animation clip
    public void SetStrongAttack()
    {
        if(weapon_left != null)
            weapon_left.SetStrongAttack();
        if(weapon_right != null)
            weapon_right.SetStrongAttack();
    }
    // called by animation clip
    public void WeaponOut()
    {
        if(weapon_left != null)
            weapon_left.WeaponOut();
        if(weapon_right != null)
            weapon_right.WeaponOut();
    }

    //// called by animator
    //public void IsAttack()
    //{
    //    isAttack = true;
    //}

    //public void AttackOut()
    //{
    //    isAttack = false;
    //}
}
