﻿using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
public class Monster : MonoBehaviour
{
    private FSM _fsm;

    [Header("Idle 설정")]
    [SerializeField]
    private float wanderRadius = 5f; // 배회 반경
    private Vector3 spawnPoint; // 스폰포인트
    public Vector3 SpawnPoint { get { return spawnPoint; } }
    public float WanderRadius { get { return wanderRadius; } }

    [Header("Chase 설정")]
    [SerializeField]
    private LayerMask obstacleLayer; // 장애물 레이어
    [SerializeField]
    private LayerMask playerLayer; // 플레이어 레이어
    [SerializeField]
    private float sightRange = 10f; // 몬스터의 시야 범위(원형)
    [SerializeField]
    private float fieldOfView = 120f; // 몬스터의 시야각
    public Transform TargetPlayer { get; private set; } // 몬스터가 추적하는 플레이어

    [Header("Attack 설정")]
    [SerializeField] float attackRange = 2f;
    [SerializeField] float basicCoolTime = 3f;
    [SerializeField] EnemyWeapon _enemyWeapon;
    public EnemyWeapon Weapon{ get {return _enemyWeapon;} } // 프로퍼티

    [Header("몬스터 스탯 설정")]
    [SerializeField] int maxHP;
    [SerializeField] int currentHP;
    [SerializeField] float attackPower = 5f;
    [SerializeField] float def = 10f;
    [SerializeField] float constant_def = 100f;
    [SerializeField] float speed = 10f;

    // IMonsterState에서 접근할 프로퍼티 설정
    public Rigidbody Rigid { get; private set; }
    public BoxCollider Collider { get; private set; }
    public Animator Anim { get; private set; }
    public NavMeshAgent Agent { get; private set; }

    Material mat;
    Color originalColor;

    private void Awake()    
    {
        Rigid = GetComponent<Rigidbody>();
        Collider = GetComponent<BoxCollider>();
        Anim = GetComponent<Animator>();
        Agent = GetComponent<NavMeshAgent>();
        _enemyWeapon.enabled = false;

        spawnPoint = gameObject.transform.position; // 스폰포인트는 몬스터의 처음 위치
    }
    void Start()
    {
        // fsm 세팅
        _fsm = new FSM(this);

        // 
    }

    void Update()
    {
        // _fsm을 통한 상태 변경 부분
        switch (_fsm.CurrentState) // fsm의 현재상태 프로퍼티 접근
        {
            case IdleState:
                if (CheckPlayerInSight()) // 시야 범위 내
                {
                    if (CheckAttackRange()) // 사정거리 안
                    {
                        SetState(new AttackState(this));
                    }
                    else // 사정거리 밖
                    {
                        SetState(new ChaseState(this));
                    }
                }
                break;

            case ChaseState:
                if (CheckPlayerInSight()) // 시야 범위 내
                {
                    if (CheckAttackRange()) // 사정거리 안
                    {
                        SetState(new AttackState(this));
                    }
                }
                else // 플레이어 시야 밖
                {
                    SetState(new IdleState(this));
                }
                break;

            case AttackState:
                if (CheckPlayerInSight()) // 시야 범위 내 
                {
                    if (!CheckAttackRange()) // 공격 범위 밖
                    {
                        SetState(new ChaseState(this));
                    }
                }
                else // 시야 범위 밖
                {
                    SetState(new IdleState(this));
                }
                break;
        }

        // _fsm을 통해 변경된 상태를 실행
        _fsm.ExecuteState();
    }

    private bool CheckAttackRange()
    {
        Vector3 directionToPlayer = TargetPlayer.position - transform.position;
        return directionToPlayer.magnitude < attackRange;
    }

    private bool CheckPlayerInSight()
    {
        // 1. sightRange(시야범위) 내 playerLayer 검출
        Collider[] colliders = Physics.OverlapSphere(transform.position, sightRange, playerLayer);
        // 2. colliders에 대해서 검사 
        foreach(var collider in colliders)
        {
            Transform player = collider.transform;
            // 몬스터->플레이어 방향벡터
            Vector3 directionToPlayer = (player.position - transform.position);
            // 방향벡터와 전방벡터 각도
            float angle = Vector3.Angle(directionToPlayer, transform.forward);

            if (angle < fieldOfView / 2) // 시야각/2 보다 작으면 플레이어가 있음을 의미
            {
                // Raycast(원점, 방향단위벡터, 최대길이, 검출 원하는 layerMask)
                if (!Physics.Raycast(transform.position, directionToPlayer.normalized, directionToPlayer.magnitude, obstacleLayer))
                {
                    TargetPlayer = player; // 타겟 설정
                    return true; // 시야 범위 내 플레이어 존재 && 장애물 없는 경우
                }
            }
        }
        TargetPlayer = null;
        return false; // 시야 범위 내 플레이어 없음 || 장애물 존재
    }

    // 상태 변경 _fsm에 위임
    private void SetState(IMonsterState state)
    {
        _fsm.SetState(state);
    }
    // 기즈모
    private void OnDrawGizmos()
    {
        // 시야 범위 기즈모
        Gizmos.color = Color.blue;
        DrawFieldOfView();

        // 공격 범위 기즈모
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // 스폰 포인트 기즈모(잘나오나 임시 체크 위함)
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(spawnPoint, wanderRadius);

    }
    private void DrawFieldOfView()
    {
        Vector3 forwardRight = Quaternion.Euler(0, fieldOfView / 2, 0) * transform.forward;
        Vector3 forwardLeft = Quaternion.Euler(0, -fieldOfView / 2, 0) * transform.forward;

        Gizmos.DrawRay(transform.position, forwardRight * sightRange);
        Gizmos.DrawRay(transform.position, forwardLeft * sightRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere (transform.position, sightRange);

    }
    // 몬스터의 체력 깎는 함수
    // 플레이어쪽에서 이를 호출해야 한다.
    public void TakeDamage(int damage,Vector3 enmenyPosition)
    {
        currentHP -= damage;

        if(IsDie())
        {
            Die();
        }
    }
    private bool IsDie()
    {
        return currentHP <= 0;
    }

    private void Die()
    {
        // 죽은 상태로 전환
        SetState(new DeadState(this));
    }
}