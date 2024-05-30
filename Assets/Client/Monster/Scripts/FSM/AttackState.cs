using UnityEditor;
using UnityEngine;
public class AttackState : IMonsterState
{
    Monster monster;
    float currentTime = 0;
    float attackInterval = 3f;
    public AttackState(Monster monster)
    {
        this.monster = monster;
    }

    public void EnterState()
    {
        Debug.Log("Attack: Enter");
        monster.transform.LookAt(monster.TargetPlayer);
        monster.Anim.SetTrigger("doAttack");
        monster.Anim.SetInteger("randomValue", Random.Range(0, 3));
    }

    public void ExitState()
    {
        Debug.Log("Attack: Exit");
    }

    public void ExecuteState()
    {
        //AnimatorStateInfo animatorStateInfo = monster.Anim.GetCurrentAnimatorStateInfo(0);
        //if (animatorStateInfo.IsName("attack1"))
        //{
        //    Debug.Log($"normalizedTime: {animatorStateInfo.normalizedTime}");
        //}
        currentTime += Time.deltaTime;
        if (currentTime >= attackInterval)
        {
            monster.transform.LookAt(monster.TargetPlayer);
            monster.TransformTrigger();
            monster.Anim.SetTrigger("doAttack");
            monster.Anim.SetInteger("randomValue", Random.Range(0, 3));
            currentTime = 0f;
            //monster.Anim.SetInteger("randomValue", 0);
            //monster.Anim.SetInteger("randomValue", 2);
        }

        // 재생중이 아닐때만 애니메이션 재생
        //if (!IsAnimationRunning(monster.Anim, "attack1"))
        //{
        //    Debug.Log("애니메이션 실행" + count);
        //    monster.Anim.SetTrigger("doAttack");
        //    monster.Anim.SetInteger("randomValue", 0);
        //    count++;
        //}
        //Debug.Log("=========================================================");
    }
    bool IsAnimationRunning(Animator animator, string animationStateName)
    {
        if (animator == null) return false;

        // 애니메이터 첫번째 레이어 반환 
        AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorClipInfo[] animatorClipInfo = animator.GetCurrentAnimatorClipInfo(0);
        if (animatorStateInfo.IsName(animationStateName)) // 매개변수로 받아온 이름의 상태이면
        {
            Debug.Log($"normalizedTime :{animatorStateInfo.normalizedTime}");
            if (animatorStateInfo.normalizedTime <= 1f)
            {

                //Debug.Log($"normalizedTime :{animatorStateInfo.normalizedTime}");
                //Debug.Log("실행중입니다.");
                return true;
            }
            else
            {
                Debug.Log("실행이 끝났습니다.");
                return false;
            }
        }
        else // 매개변수로 받아온 이름의 상태가 아니면
        {
            Debug.Log($"애니메이션 종류: {animator.GetCurrentAnimatorClipInfo(0)[0].clip.name}");
            Debug.Log($"애니메이션 길이: {animator.GetCurrentAnimatorClipInfo(0)[0].clip.length}");
            return false; // 아니면 false
        }
        //if(animatorStateInfo.IsName(animationStateName) && (animatorStateInfo.normalizedTime % 1.0f) < 1f)
        //{
        //    return true;
        //}
        //else
        //{
        //    return false;
        //}
    }

}
//monster.Anim.SetTrigger("doAttack");
//monster.Anim.SetInteger("randomValue", Random.Range(0,2));

//if (monster.AttackController.isAttack)
//{
//    return;
//}

//if (monster.Anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.8) //개선필요
//{
//    randomValue = Random.Range(0, 2);
//    monster.Anim.SetTrigger("doAttack");
//    monster.Anim.SetInteger("randomValue", randomValue);
//}