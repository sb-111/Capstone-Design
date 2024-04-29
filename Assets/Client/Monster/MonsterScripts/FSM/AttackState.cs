using UnityEditor;
using UnityEngine;
public class AttackState : IMonsterState
{
    Monster monster;
    MonsterAttackCTRL attackController;
    int randomValue;
    public AttackState(Monster monster)
    {
        this.monster = monster;
        attackController = monster.AttackController;
    }

    public void EnterState()
    {
        if (attackController.isAttack)
        {
            return;
        }

        randomValue = Random.Range(0, 2);
        monster.Anim.SetTrigger("doAttack");
        monster.Anim.SetInteger(randomValue, randomValue);
    }

    public void ExitState()
    {

    }

    public void ExecuteState()
    {
        if (attackController.isAttack)
        {
            return;
        }
       
        if (monster.Anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.8) //개선필요
        {
            randomValue = Random.Range(0, 2);
            monster.Anim.SetTrigger("doAttack");
            monster.Anim.SetInteger("randomValue", randomValue);
        }
    }
    bool IsAnimationRunning(Animator animator, string animationStateName)
    {
        if (animator == null) return false;

        // 애니메이터 첫번째 레이어 반환 
        AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (animatorStateInfo.IsName(animationStateName)) // 매개변수로 받아온 이름의 상태이면
        {
            return animatorStateInfo.normalizedTime < 1f; // 실행 중이면 true 반환
        }
        return false; // 아니면 false
    }

}