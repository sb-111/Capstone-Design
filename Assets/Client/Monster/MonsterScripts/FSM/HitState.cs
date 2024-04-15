using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitState : MonoBehaviour, IMonsterState
{

    private Monster monster;

    public HitState(Monster monster)
    {
        this.monster = monster;
    }


    // 1. 최초 진입 시 1회 실행
    public void EnterState()
    {
        //피격 애니메이션 실행 - 현재 애니메이션 없음
        monster.Anim.SetTrigger("getHit");
    }
    
    //2. 반복 실행
    public void ExecuteState()
    {
        //비어있음
    }

    public void ExitState()
    {
        //비어있음
    }
}
