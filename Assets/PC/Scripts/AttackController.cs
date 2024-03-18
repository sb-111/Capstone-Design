using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    //어쌔신 한정
    public Weapon weapon_left;
    public Weapon weapon_right;

    Animator anim;
    Player player_controller;
    PlayerStatus state;
    // Start is called before the first frame update
    private void Awake()
    {
        anim = GetComponent<Animator>();
        state= GetComponent<PlayerStatus>();
        player_controller = GetComponent<Player>();
    }


    public void attack1()
    {
        StartCoroutine(coAttack1());
    }

    IEnumerator coAttack1()
    {
        anim.SetTrigger("doLattack");
        weapon_left.Use(1.05f);         //두 무기 동시에 쓰는 공격임
        weapon_right.Use(1.05f);
        //attackDelay = 0;                여기서 초기화 하면 연속 두번 입력됨

        bool animationState = false;
        while (!animationState)
        {
            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("atack11") && stateInfo.normalizedTime >= 0.8f)       //0.8f = 애니메이션의 HasExitTime
            {
                animationState = true;
            }
            yield return null;
        }
        player_controller.attackOut();
    }


    public void attack2()
    {
        StartCoroutine(coAttack2());
    }

    IEnumerator coAttack2()         //좌클릭 공격 /이 코루틴 내에서 시작 시간 설정하고 Use에 해당 공격의 무기 트리거 종료 시간을 넘기자.
    {
        //isAttack = true;
        anim.SetTrigger("doRattack");
        weapon_right.Use(0.15f);
        bool animationState = false;
        while (!animationState)
        {
            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("atack16") && stateInfo.normalizedTime >= 0.95f)
            {
                animationState = true;
            }
            yield return null;
        }
        player_controller.attackOut();
    }

    public void strongAttack()
    {
        StartCoroutine(coStrongAttack());
    }

    IEnumerator coStrongAttack()
    {
        //isAttack = true;
        anim.SetTrigger("doStrongAttack");
        yield return new WaitForSeconds(0.6f);          //생각한 것 보다 0.01초 차이로 빠르게 켜지거나 늦게 켜짐
        weapon_left.Use(0.3f);
        yield return new WaitForSeconds(0.26f);         //0.14s late
        weapon_right.Use(0.6f);
        yield return new WaitForSeconds(0.85f);         //0.008s late
        weapon_right.Use(0.31f);
        weapon_left.Use(0.31f);
        yield return new WaitForSeconds(0.63f);         //0.001s fast
        weapon_left.Use(0.35f);
        //attackDelay = 0;
        bool animationState = false;
        while (!animationState)
        {
            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("atack8") && stateInfo.normalizedTime >= 0.8f)
            {
                animationState = true;
            }
            yield return null;
        }
        player_controller.attackOut();
    }
}
