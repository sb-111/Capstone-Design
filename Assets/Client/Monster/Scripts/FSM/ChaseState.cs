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
        Debug.Log("Chase: Enter");
        monster.Agent.isStopped = false;
    }

    public void ExitState()
    {
        Debug.Log("Chase: Exit");
        monster.Anim.SetBool("Run", false);
        monster.Agent.isStopped = true;
    }

    public void ExecuteState()
    {
        //Debug.Log("Chase: 진행중");
        monster.Anim.SetBool("Run", (monster.Agent.velocity.magnitude >= 0.05f) ? true : false);
        if (monster.TargetPlayer != null)
        {
            monster.Agent.SetDestination(monster.TargetPlayer.position);
        }
    }
}