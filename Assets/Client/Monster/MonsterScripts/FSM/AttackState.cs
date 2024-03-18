using UnityEditor;
using UnityEngine;
public class AttackState : IMonsterState
{
    Monster monster;
    public AttackState(Monster monster)
    {
        this.monster = monster;
    }

    public void EnterState()
    {
        Debug.Log("Attack State 진입");
        // 1. 웨폰 스크립트 활성화
        monster.Weapon.enabled = true;

        // 2. 공격 애니메이션 실행
        monster.Anim.SetTrigger("doAttack");
        // 3. 실제 공격
        monster.Weapon.Use(1.6f); // 1.6초 동안은 collider를 켜라
    }

    public void ExitState()
    {
        Debug.Log("Attack State 탈출");
        // 1. 웨폰 스크립트 비활성화
        monster.Weapon.enabled = false;
    }
    // 공격 상태인 동안 계속 호출
    public void ExecuteState()
    {
        Debug.Log("Attack State 진행중");
        if (!IsAnimationRunning(monster.Anim, "HornAttack"))
        {
            Debug.Log("계속 부르고 있니");
            // 1. 공격 애니메이션 실행
            monster.Anim.SetTrigger("doAttack");
            // 2. 실제 공격
            monster.Weapon.Use(1);
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