using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseState : IMonsterState
{
    private Monster monster;
    private GameObject target;
    public DefenseState(Monster monster)
    {
        this.monster = monster;
    }


    public void EnterState()
    {
        Debug.Log("Defense: Enter");
        monster.Agent.isStopped = false;
    }

    public void ExitState()
    {
        Debug.Log("Defense: Exit");
        monster.Anim.SetBool("Run", false);
        monster.Agent.isStopped = true;
    }

    public void ExecuteState()
    {
        //Debug.Log("Chase: ÁøÇàÁß");
        target = GameObject.FindWithTag("Portal");
        monster.Anim.SetBool("Run", (monster.Agent.velocity.magnitude >= 0.05f) ? true : false);
        if (target != null)
        {
            monster.Agent.SetDestination(target.transform.position);
        }
    }
}
