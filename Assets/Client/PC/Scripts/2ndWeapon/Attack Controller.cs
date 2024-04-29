using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    // 전사는 weapon_right만 사용 ******** 어쌔신은 둘 다 사용
    [Header("수동 설정")]
    public Weapon weapon_left;
    public Weapon weapon_right;

    [Header("자동 설정")]
    public CameraShake cameraShaking;
    Animator anim;
    Player player_controller;
    
   
    private void Awake()
    {
       anim = GetComponent<Animator>();
       player_controller = GetComponent<Player>();
       cameraShaking = Camera.main.GetComponent<CameraShake>();
        
    }

    private void Start()
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
        /*
        yield return new WaitForSeconds(0.58f);

        cameraShaking.Zoom(0.55f,0.638f,0.0f,10.0f);

        yield return new WaitForSeconds(1.18f);
        cameraShaking.Shaking(0.5f,7.5f);
        */
        yield return null;
    }

    public void Parrying() // 즉시 패링 애니메이션
    {
        anim.SetTrigger("doParrying");
    }





    //공격 카메라 효과
    public void ShakeCamera(float a = 0.5f, float b = 2.0f)
    {
        cameraShaking.Shaking(a, b);
    }
    public void ZoomCamera(float a = 0.36f, float b = 0.2f , float c = 0.0f , float d = 10.0f)
    {
        cameraShaking.Zoom(a,b,c,d);
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

    public void IsHeavyAttack()
    {
        weapon_right.isHeavyAttack = true;
        Debug.Log("강공격 상태: "+weapon_right.isHeavyAttack);
    }
}
