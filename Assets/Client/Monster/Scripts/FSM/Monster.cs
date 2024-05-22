using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using Unity.VisualScripting;

public class Monster : MonoBehaviour
{
    private FSM _fsm;
    public enum MonsterType { Cyclops, Goblin, Hobgoblin, Kobold, Troll }

    [Header("몬스터 타입 설정")]
    [SerializeField] private MonsterType _monsterType;
    public MonsterType Type {  get { return _monsterType; }  }

    [Header("Idle 설정")]
    [SerializeField]
    private float wanderRadius = 5f; // 배회 반경
    private Vector3 spawnPoint; // 스폰포인트
    public float WanderRadius { get { return wanderRadius; } }
    public Vector3 SpawnPoint { get { return spawnPoint; } }

    [Header("Chase 설정")]
    [SerializeField]
    private LayerMask obstacleLayer; // 장애물 레이어
    [SerializeField]
    private LayerMask playerLayer; // 플레이어 레이어
    [SerializeField]
    private float sightRange = 10f; // 몬스터의 시야 범위(원형)
    [SerializeField]
    private float fieldOfView = 120f; // 몬스터의 시야각
    public Transform TargetPlayer { get; set; } // 몬스터가 추적하는 플레이어

    [Header("Attack 설정")]
    [SerializeField] float attackRange = 2f;
    [SerializeField] float basicCoolTime = 3f;
    //[SerializeField] EnemyWeapon _enemyWeapon;
    //public EnemyWeapon Weapon{ get {return _enemyWeapon;} } // 프로퍼티

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
    public MonsterAttackCTRL AttackController { get; private set; }
    public MonsterSound MSound { get; private set; }

    Material mat;
    Color originalColor;
    [Header("몬스터 material 설정")]
    [SerializeField] GameObject skin_30p;
    [SerializeField] GameObject skin_70p;


    [Header("몬스터 Drop 설정")]
    [SerializeField] GameObject soul;
    //패링 포인트
    public bool weakPoint= false;

    [Header("몬스터 공격시 트리거 전환 설정")]
    [SerializeField] BoxCollider[] ArmCollider;
    [SerializeField] CapsuleCollider bodyCollider;


    private void Awake()    
    {
        Rigid = GetComponent<Rigidbody>();
        Collider = GetComponent<BoxCollider>();
        Anim = GetComponent<Animator>();
        Agent = GetComponent<NavMeshAgent>();
        AttackController = GetComponent<MonsterAttackCTRL>();
        spawnPoint = gameObject.transform.position; // 스폰포인트는 몬스터의 처음 위치
        MSound = GetComponent<MonsterSound>();
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
                //IsMoving();
                break;

            case ChaseState:
                if (CheckPlayerInSight()) // 시야 범위 내
                {
                    if (CheckAttackRange()) // 사정거리 안
                    {
                        SetState(new AttackState(this));
                    }
                }
                else // 시야 범위 밖
                {
                    SetState(new IdleState(this));
                }
                //IsMoving();
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

            case HitState:
                if (Anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.95f)
                {
                    return;                     //피격 애니메이션 실행률 95%이하면 X
                }
                else // Hit -> Chase
                {
                    SetState(new ChaseState(this));
                    // Chase ->
                    // 시야밖: Idle
                    // 시야 범위 내 사정거리 밖: Chase
                    // 시야 범위 내 사정거리 안: Attack
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
        //TargetPlayer = null;
        return false; // 시야 범위 내 플레이어 없음 || 장애물 존재
    }

    // 상태 변경 _fsm에 위임
    private void SetState(IMonsterState state)
    {
        _fsm.SetState(state);
    }
    
    // 몬스터의 체력 깎는 함수
    // 플레이어쪽에서 이를 호출해야 한다.
    /// <summary>
    /// 몬스터의 체력 깎는 메서드: 플레이어가 호출
    /// </summary>
    /// <param name="damage">데미지</param>
    /// <param name="enmenyPosition">?</param>
    public void TakeDamage(int damage)
    {

        Debug.Log("TakeDamage() 호출");
        currentHP -= damage;

        // 9버전 추가 - 후방 공격 시 플레이어쪽으로 바로 돌아보게 하기 위함 + 타게팅 설정까지
        if(TargetPlayer == null)
        {
            Vector3 vec = transform.forward * -1f;
            transform.LookAt(vec);
        }
        CheckPlayerInSight(); // 강공격 당할 때 필요함(Idle -> Hit -> Chase에서 타게팅 과정이 없으므로)

        float hpPercentage = currentHP / (float)maxHP;

        if (IsDie())
        {
            Die();
        }

        //if (hpPercentage <= 0.7 && hpPercentage > 0.3)
        //{
        //    skin_70p.SetActive(true);
        //}
        //if (hpPercentage <= 0.3)
        //{
        //    skin_70p.SetActive(false);
        //    skin_30p.SetActive(true); 
        //}

    }

    public void Parried()       //패링 당해서 스턴 상태 부여
    {
        if (weakPoint)
        {
            Anim.SetTrigger("Parried");
            StunEffect stun = this.AddComponent<StunEffect>();
            stun.Duration = 5.0f; //스턴 시간
            stun.OnStart();

            Debug.Log("패링당했습니다.");
        }
    }



    private bool IsDie()
    {
        return currentHP <= 0;
    }
    /// <summary>
    /// DeadState로 바로 전환하는 메서드
    /// </summary>
    private void Die()
    {
        Debug.Log("Die() 호출");
        float height = 2.0f;
        Vector3 dropVec = new Vector3(transform.position.x, transform.position.y + height, transform.position.z);
        GameObject droppedSoul =  Instantiate(soul, dropVec, Quaternion.identity);
        SetState(new DeadState(this));
    }

    /// <summary>
    /// HitState로 바로 전환하는 메서드
    /// </summary>
    /// <param name="cctime"></param>
    public void HitResponse(float cctime = 1.0f)       //강공격에 의한 피격 반응 애니메이션 출력(cc기 시간)
    {
        if(TargetPlayer == null)
        {
            Debug.Log("타겟없음");
        }
        else
        {
            Debug.Log($"힛리스폰즈: {TargetPlayer.name}");
            
        }
        SetState(new HitState(this));
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
    private void IsMoving()
    {
        if (Agent.velocity.magnitude >= 0.05f)
        {
            switch (_fsm.CurrentState)
            {
                case IdleState: // 각 state 구현으로 옮겨도 될듯
                    Anim.SetBool("Walk", true);
                    Anim.SetBool("Run", false);
                    break;

                case ChaseState:
                    Anim.SetBool("Run", true);
                    Anim.SetBool("Walk", false);
                    break;

                case AttackState:
                    Anim.SetBool("Run", true);;
                    Anim.SetBool("Walk", false);
                    break;
            }
        }
        else
        {
            Anim.SetBool("Walk", false);
            Anim.SetBool("Run", false);
        }
    }


    public void TransformTrigger()
    {
        bodyCollider.isTrigger = true;
        for (int i =0;i<ArmCollider.Length;i++)
        {
            ArmCollider[i].isTrigger = true;
        }
        
    }

    public void TrasnformTriggerOut()
    {
        for (int i = 0; i < ArmCollider.Length; i++)
        {
            ArmCollider[i].isTrigger = false;
        }
        bodyCollider.isTrigger = false;
    }
}