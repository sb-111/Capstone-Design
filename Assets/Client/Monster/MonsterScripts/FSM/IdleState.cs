using System;
using UnityEngine;
using UnityEngine.AI;

public class IdleState : MonoBehaviour, IMonsterState
{
    Monster monster;
    private float timer;
    private float wanderTimer = 5f;
    public IdleState(Monster monster)
    {
        this.monster = monster;
        timer = 0f;
    }

    public void EnterState()
    {
        Debug.Log("Idle State 진입");
        monster.Agent.isStopped = false; // Agent 활성화
    }

    public void ExitState()
    {
        Debug.Log("Idle State 탈출");
        monster.Agent.isStopped = true;
    }

    public void ExecuteState()
    {
      //  Debug.Log($"Idle 수행중, 현재 타이머: {timer}");
        timer += Time.deltaTime;
        if (timer >= wanderTimer)
        {
           // Debug.Log("위치 전환");
            Vector3 newPos = SetRandomPosInSpawnPointRange(monster.SpawnPoint.position, monster.WanderRadius, -1);
            monster.Agent.SetDestination(newPos);
            timer = 0f;
        }
    }

    private Vector3 SetRandomPosInSpawnPointRange(Vector3 spawnPoint, float range, int layermask)
    {
        // 1. 범위내 랜덤 구 위치 벡터 
        Vector3 randDir = UnityEngine.Random.insideUnitSphere * range; ;
        randDir += spawnPoint; // 스폰 원점 벡터 더하기 

        NavMeshHit navHit;
        // randDir 주변 네비게이션 메쉬 위 가장 가까운 점 찾아 navHit에 저장
        NavMesh.SamplePosition(randDir, out navHit, range, layermask); // layermask가 1이면 모든 레이어 대상

        return navHit.position;
    }
  
}