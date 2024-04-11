using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    //콤보 어택
    int comboStep = 1;
    float comboTimer = 0.0f;
    private bool isAttackLock;//2연속 공격 입력 방지용 플래그 추가
    bool ya=false;

    //어쌔신 한정
    public Weapon weapon_left;
    public Weapon weapon_right;

    Animator anim;
    Player player_controller;
    PlayerStatus state;
    public CameraShake cameraShaking;
   
  
    public GameObject WeaponPoint;
        
    // Start is called before the first frame update
    private void Awake()
    {
        anim = GetComponent<Animator>();
        state = GetComponent<PlayerStatus>();
        player_controller = GetComponent<Player>();
        cameraShaking = Camera.main.GetComponent<CameraShake>();
        
    }

    private void Update()
    {

    }
    public void attack1()
    {
        StartCoroutine(coAttack1());
        
    }
    
    IEnumerator coAttack1()
    {
        anim.SetTrigger("doRattack");
        anim.SetBool("isRattack", true);
        yield return null;
        
    }


    public void attack2()
    {
        StartCoroutine(coAttack2());
    }

    IEnumerator coAttack2()       
    {
        anim.SetTrigger("doLattack");
        anim.SetBool("isLattack", true);
        yield return null;
    }

    public void strongAttack()
    {
        if(player_controller.isAttack) { return; }
        StartCoroutine(coStrongAttack());
    }

    IEnumerator coStrongAttack()
    {
        anim.SetTrigger("doStrongAttack");
        yield return new WaitForSeconds(0.58f);

        cameraShaking.Zoom(0.55f,0.638f,0.0f,15.0f);

        yield return new WaitForSeconds(1.18f);
        cameraShaking.Shaking(1.0f,7.5f);
    }

    public void Parrying() // 즉시 패링 애니메이션
    {
        anim.SetTrigger("doParrying");
    }





    //공격 카메라 효과
    public void ShakeCamera()
    {
        cameraShaking.Shaking(0.5f, 2.0f);
    }
    public void ZoomCamera()
    {
        cameraShaking.Zoom(0.36f,0.2f,0.0f,5.0f);
    }


    //공격&방어 이펙트
    public void SwordEffect(int value = 0)
    {
        if (value != 0)
        {
            bool reverse = false;
            weapon_right.EffectInstance(reverse);
        }
        else
        {
            bool forward = true;
            weapon_right.EffectInstance(forward);
        }
    }

    public void ShieldEffect()
    {
        weapon_right.ShieldEffectInstance();
    }

    public void StrongEffect()
    {
        weapon_right.StrongEffectInstance();
    }
}
