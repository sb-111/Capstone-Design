using System.Collections;
using UnityEngine;

public class DeadState : MonoBehaviour, IMonsterState
{
    private Monster monster;
    public DeadState(Monster monster)
    {
        this.monster = monster;
    }
    // 1. 들어올 때 한번 실행
    public void EnterState()
    {
        Debug.Log("DeadState 진입");
        // Dead 애니메이션 실행
        monster.Anim.SetTrigger("doDie");
        Destroy(monster.gameObject, 1f);
    }
    // 실행 X
    public void ExecuteState()
    {
        
    }

    // 실행 X
    public void ExitState()
    {

    }
}
