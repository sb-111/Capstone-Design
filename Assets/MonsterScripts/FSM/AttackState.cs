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
    }

    public void ExitState()
    {
        Debug.Log("Attack State 탈출");
    }

    public void ExecuteState()
    {
        Debug.Log("Attack State 진행중");
    }
}