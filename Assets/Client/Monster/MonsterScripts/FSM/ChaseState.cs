using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : IMonsterState
{
    private Monster monster;
    

    public ChaseState(Monster monster)
    {
        this.monster = monster;
    }

    public void EnterState()
    {
        //Debug.Log("Chase State 진입");
        monster.Agent.isStopped = false;
    }

    public void ExitState()
    {
        //Debug.Log("Chase State 탈출");
        monster.Agent.isStopped = true;
    }

    public void ExecuteState()
    {
        //Debug.Log("Chase State 진행중");
        monster.Agent.SetDestination(monster.TargetPlayer.position);
    }
}