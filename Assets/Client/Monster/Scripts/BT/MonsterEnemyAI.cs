using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class MonsterEnemyAI : MonoBehaviour
{
    Animator animator;
    IBTNode behaviourTree;

    [Header("Combat")]
    [SerializeField] float attackRange;
    [SerializeField] Transform target;

    [Header("MoveToSpawn")]
    [SerializeField] float maxMoveRange;
    [SerializeField] Vector3 originPos;
    bool isInvincible;
    bool isMovingToSpawnPoint = false;
    [SerializeField] Transform spawnPoint;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float spawnPointBoundary;

    [Header("MoveToPC")]
    [SerializeField] LayerMask playerLayer;
    [SerializeField] float fieldOfViewAngle; // 시야각
    [SerializeField] float viewRadius; // 시야 범위
    List<Transform> targetList = new List<Transform>();   
    private void Awake()
    {
        animator = GetComponent<Animator>();
        behaviourTree = InitBehaviorTree(); 
    } 
    IBTNode InitBehaviorTree()
    {
        var attackCondition = new ActionNode(IsAttack);
        var attackRangeCondition = new ActionNode(IsAttackRange);
        var attack1Action = new ActionNode(Attack1);
        //var attack2Action = new ActionNode(Attack2);
        //var attack3Action = new ActionNode(Attack3);
        var attackSelector = new SelectorNode(new List<IBTNode>() { attack1Action, /*attack2Action, attack3Action */});
        var combatSequence = new SequenceNode(new List<IBTNode>() { attackCondition, attackRangeCondition, attackSelector });

        var moveRangeCondition = new ActionNode(IsOutOfMoveRange);
        var initTargetAction = new ActionNode(InitTarget);
        var invincibilityAction = new ActionNode(MakeInvincible);
        var moveToSpawnPointAction = new ActionNode(MoveToSpawnPoint);
        var moveToSpawnPointSequence = new SequenceNode(new List<IBTNode>() { moveRangeCondition, initTargetAction, invincibilityAction, moveToSpawnPointAction });

        var pcInFieldOfVisionCondition = new ActionNode(IsPCInFieldOfVision);
        var targetListCondition = new ActionNode(IsTargetList);
        var registrationTargetListAction = new ActionNode(RegisterTargetList);
        var moveToPCAction = new ActionNode(MoveToPC);
        var targetRegistrationSequence = new SequenceNode(new List<IBTNode>() { targetListCondition, registrationTargetListAction});
        var moveToPCSequence = 
            new SequenceNode(
                new List<IBTNode>() { pcInFieldOfVisionCondition,
                    new SelectorNode(
                        new List<IBTNode>(){targetRegistrationSequence, moveToPCAction})});

        var idleAction = new ActionNode(IdleMonster);

        var rootSelector = new SelectorNode(new List<IBTNode>() { combatSequence, moveToPCSequence, moveToSpawnPointSequence, idleAction});

        return rootSelector;
         
    }

   

    bool IsAnimationRunning(Animator animator, string animationStateName)
    {
        if(animator == null) return false;

        // 애니메이터 첫번째 레이어 반환 
        AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (animatorStateInfo.IsName(animationStateName)) // 매개변수로 받아온 이름의 상태이면
        {
            return animatorStateInfo.normalizedTime < 1f; // 실행 중이면 true 반환
        }
        return false; // 아니면 false
    }

    #region attackSequence
    /*
     * 공격 애니메이션 실행 중인지 나타내는 함수
     * 공격 중이면 Running
     * 아니면 Success
     */
    private IBTNode.NodeState IsAttack()
    {
        if (IsAnimationRunning(animator, "Attack"))
        {
            return IBTNode.NodeState.Running;
        }
        return IBTNode.NodeState.Success;
    }
    /*
     * 공격 대상이 공격 범위 안에 있는지 확인하는 함수
     */
    private IBTNode.NodeState IsAttackRange()
    {
        if (target == null) return IBTNode.NodeState.Failure;
        float sqrDistance = (transform.position -  target.position).sqrMagnitude;
        if(sqrDistance <= attackRange * attackRange)
        {
            return IBTNode.NodeState.Success;
        }
        else
        {
            return IBTNode.NodeState.Failure;
        }
    }
    /*
     * 공격 애니메이션을 실행 하는 함수
     * 실제 공격 판정은 해당 몬스터가 가진 데미지를 가지고 플레이어쪽에서 처리한다.
     */
    private IBTNode.NodeState Attack1()
    {
        if (target == null) return IBTNode.NodeState.Failure; // null이면 실패
        animator.SetTrigger("Attack1");
        return IBTNode.NodeState.Success; // null 아니면 성공

    }
    #endregion

    #region moveToSpawnPointSequence
    /*
     * 최대 이동 가능 범위를 벗어났는지 확인
     */
    private IBTNode.NodeState IsOutOfMoveRange()
    {
        // 스폰 포인트로 이동 중이라면 최대 이동 가능 범위 체크를 무시
        if (isMovingToSpawnPoint)
        {
            return IBTNode.NodeState.Success;
        }

        float sqrDistance = (originPos - transform.position).sqrMagnitude;
        if(sqrDistance <= maxMoveRange * maxMoveRange) // 범위 내
        {
            return IBTNode.NodeState.Failure;
        }
        else // 범위 초과
        {
            return IBTNode.NodeState.Success;
        }
    }
    /*
     * 타겟 초기화
     */
    private IBTNode.NodeState InitTarget()
    {
        if (isMovingToSpawnPoint)
        {
            // 스폰 포인트로 이동 중이라면, 타겟 초기화는 성공적으로 처리된 것으로 간주
            return IBTNode.NodeState.Success;
        }

        if (target != null)
        {
            target = null;
            return IBTNode.NodeState.Success;
        }
        return IBTNode.NodeState.Failure;
    }
    /*
    * 무적화
    */
    private IBTNode.NodeState MakeInvincible()
    {
        if (isMovingToSpawnPoint)
        {
            // 스폰 포인트로 이동 중이라면, 무적화도 성공적으로 처리된 것으로 간주
            isInvincible = true;
            return IBTNode.NodeState.Success;
        }

        isInvincible = true;
        return IBTNode.NodeState.Success;
    }
    /*
    * 스폰 위치까지 이동
    */
    private IBTNode.NodeState MoveToSpawnPoint()
    {
        isMovingToSpawnPoint = true; // 이동 시작 플래그 설정

        // 스폰포인트 방향으로 이동
        transform.position = Vector3.MoveTowards(transform.position, spawnPoint.position, moveSpeed * Time.deltaTime);
        
        // 스폰포인트 경계로 들어오면 성공
        if (Vector3.Distance(transform.position, spawnPoint.position) <= spawnPointBoundary)
        {
            // 이동 시작 플래그 초기화
            isMovingToSpawnPoint = false;
            return IBTNode.NodeState.Success;
        }
        return IBTNode.NodeState.Running; // 아니면 진행중
    }
    #endregion

    #region moveToPCSeqeunce
    /*
     * 시야 안에 PC가 존재하면 Success
     * 아니면 Failure
     */
    private IBTNode.NodeState IsPCInFieldOfVision()
    {
        // 반경 내 모든 플레이어 찾기
        Collider[] playersInRadius = Physics.OverlapSphere(transform.position, viewRadius, playerLayer);
        foreach (var playerCollider in playersInRadius)
        {
            Transform player = playerCollider.transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized; // 방향벡터

            // 방향 벡터와 몬스터의 전방 벡터 사이의 각도를 계산.
            float angleToPlayer = Vector3.Angle(transform.forward, dirToPlayer);

            // 각도가 시야각 내인 경우 성공
            if (angleToPlayer < fieldOfViewAngle / 2f)
            {
                return IBTNode.NodeState.Success;
            }
        }

        // 시야각 내에 플레이어가 없는 경우 실패
        return IBTNode.NodeState.Failure;
    }
    /*
     * 타겟 리스트에 플레이어 존재하는지 확인
     * 존재하지 않으면 성공
     * 존재하면 실패
     */
    private IBTNode.NodeState IsTargetList()
    {
        if(targetList.Count > 0) // 있으면 실패
        {
            return IBTNode.NodeState.Failure;
        }
        else // 없으면 성공
        {
            return IBTNode.NodeState.Success;
        }
    }
    /*
     * 타겟 리스트에 등록
     * 시야각 내에 들어온 플레이어를 리스트에 추가 후 성공 반환
     * 한명도 없으면 실패 반환
     */
    private IBTNode.NodeState RegisterTargetList()
    {
        // 반경 내 모든 플레이어 찾기
        Collider[] playersInRadius = Physics.OverlapSphere(transform.position, viewRadius, playerLayer);
        foreach (var playerCollider in playersInRadius)
        {
            Transform player = playerCollider.transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized; // 방향벡터

            // 방향 벡터와 몬스터의 전방 벡터 사이의 각도를 계산.
            float angleToPlayer = Vector3.Angle(transform.forward, dirToPlayer);

            // 시야각 내에 들어온 플레이어 목록에 추가
            if (angleToPlayer < fieldOfViewAngle / 2f)
            {
                targetList.Add(player);
            }
        }
        if(targetList.Count > 0) // 타겟 리스트에 어느 하나라도 들어갔다면 성공
        {
            return IBTNode.NodeState.Success;
        }
        else // 없으면 실패
        {
            return IBTNode.NodeState.Failure;
        }
    }

    private IBTNode.NodeState MoveToPC()
    {
        throw new NotImplementedException();
    }
    #endregion
    private IBTNode.NodeState IdleMonster()
    {
        throw new NotImplementedException();
    }
}
//return new SelectorNode
//(
//    new List<IBTNode>()
//    {
//        new SequenceNode // 1
//        (
//            new List<IBTNode>()
//            {
//                new ActionNode(),
//                new ActionNode(),
//                new SelectorNode()
//                {

//                }
//            }
//        ),
//        new SequenceNode() // 2
//        {

//        }, 
//        new SequenceNode() // 3
//        {

//        }, 
//        new SequenceNode() // 4
//        {

//        }
//    }

//);