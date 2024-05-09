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
        //피격 애니메이션 실행
        monster.Anim.SetTrigger("getHit");
        switch (monster.Type)
        {
            case Monster.MonsterType.Cyclops:
                monster.Anim.SetInteger("randomValue", Random.Range(0, 2));
                break;
            case Monster.MonsterType.Hobgoblin:
                monster.Anim.SetInteger("randomValue", Random.Range(0, 2));
                break;
            case Monster.MonsterType.Troll:
                monster.Anim.SetInteger("randomValue", Random.Range(0, 2));
                break;
        }
        Debug.Log("피격 진입");

    }
    
    //2. 반복 실행
    public void ExecuteState()
    {
        //비어있음
    }

    public void ExitState()
    {
        //비어있음
        Debug.Log("피격 탈출");
    }
}
