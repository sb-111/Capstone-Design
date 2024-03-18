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
        // Dead 애니메이션 실행
        monster.Anim.SetTrigger("doDie");
        Destroy(monster.gameObject, 5f);
    }
    // 2. 반복 실행
    public void ExecuteState()
    {
        // 몬스터 투명화되면서 없애기
        
    }

    // 여기선 실행 안됨
    public void ExitState()
    {

    }

    
}
