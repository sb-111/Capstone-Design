using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UIElements;
using static PlayerStatus;

public class Player : MonoBehaviourPun
{
    float hAxis;
    float vAxis;
    float mouseValueX;
    float mouseValueY;
    int speed;
    float hSensivity = 1f; // 수평 감도
    float vSensivity = 1f; // 수직 감도
    Vector3 moveVec;
    Vector3 jumpVec;
    bool isDefenseCool = false;

    bool rDown;                                             //달리기 키
    bool jDown;                                             //구르기 키
    bool isDeath;                                           //죽는 중인가?
    //bool kDown;                                             // 스탯창 버튼 눌렀는가
    [HideInInspector]
    public bool isJump;                                     //구르는 중인가?
    [HideInInspector]
    public bool dDown;                                      //방어 키
    bool dUp;
    [HideInInspector]
    public bool isDefense;                                  //방어 중인가?
    [HideInInspector]
    public bool isCC = false;                               //CC 상태인가?
    bool hpRecover;
    int hpItem = 3;

    //공격
    bool left_attack;                       //좌클릭 공격
    bool right_attack;                      //우클릭 공격
    bool strong_attack;                     //좌+우클릭 공격 합친거
    public bool isAttack = false;           //공격 중?

    bool canAttack;

    private GrowthSystem growthSystem;
    //[SerializeField] private UISoul uiSoul;
    //[SerializeField] private UIStatus uiStatus;

    bool downParryingSkill;                 //패링전용 스킬 키
    bool isParrying = false;
    bool ParryingCoolTime = false;


    [HideInInspector] public AttackController attack_controller;
    public Animator anim { get; private set; }
    public Rigidbody rigid { get; private set; }
    [HideInInspector] public PlayerStatus state;
    CharacterController characterController;
    public CharacterSound characterSound { get; private set; }
    Vector3 movements;
    //장애물 감지 
    RaycastHit hit;                     //장애물 감지를 위한 Raycast 추가
    float distanceToObstacle = 0.5f;    //장애물 감지 거리 0.5f;
    float angle;                  //감지할 각도
    float blockAngle=90.0f;
    Vector3 leftRayDirection;
    Vector3 rightRayDirection;
    bool isObstacleDetected;
    //충돌처리 방지
    [SerializeField] private LayerMask enemyLayerMask;
    [SerializeField] private float capsuleHeight = 1.7f;
    [SerializeField] private float capsuleRadius = 0.32f;


    private bool canReceiveInput = true; // 스탯창 활성화 시 인풋 막는 용도
    public bool CanReceiveInput { set { canReceiveInput = value; } }
    public float VRotation { get; private set; } // 수직 회전 값

    public delegate void ItemChangedHandler(int count);
    public event ItemChangedHandler OnPotionChanged;

    // Start is called before the first frame update
    private void Awake()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        state = GetComponent<PlayerStatus>();
        attack_controller = GetComponent<AttackController>();
        growthSystem = GetComponent<GrowthSystem>();
        characterController = GetComponent<CharacterController>();
        characterSound = GetComponent<CharacterSound>();

    }
    void Start()
    {
        speed = state.moveStats.speed;
        OnPotionChanged(hpItem); // 포션 개수 UI
    }

    // Update is called once per frame
    void Update()
    {

        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }
        if (state.basicStats.hp <= 0) { Death(); }
        if (!canReceiveInput) return;
        GetInput();
        MouseRotate();
        Move();
        Jump();
        attack_controll();
        Defenssing();
        //CkeckUI();
    }
    void attack_controll()                              //공격 입력 관리
    {
        if (!isJump && !isDeath)
        {
            if (left_attack)
            {
                attack2();
            }
            else if (right_attack)
            {
                attack1();
            }
            else if (strong_attack)
            {
                strongAttack();
            }
            else if (downParryingSkill)
            {
                ParryingSkill();
            }
        }
    }
    void GetInput()                                                         //   사용자 키보드 & 마우스 입력 관리
    {
        mouseValueX = Input.GetAxis("Mouse X");                             // 마우스 수평 회전 값
        mouseValueY = Input.GetAxis("Mouse Y");                             // 마우스 수직 회전 값


        if (isCC) { return; }                                               // !! CC기 걸리면 다른 인풋 무시 !!
        hAxis = Input.GetAxisRaw("Horizontal");                             // x축 이동(-1/1)
        vAxis = Input.GetAxisRaw("Vertical");                               // z축 이동(-1/1)
        rDown = Input.GetKey(KeyCode.LeftShift);                            //leftshift
        jDown = Input.GetKeyDown(KeyCode.Space);                            //spacebar
        left_attack = Input.GetMouseButtonDown(0);
        right_attack = Input.GetMouseButtonDown(1);
        strong_attack = Input.GetMouseButtonDown(2);
        dDown = Input.GetKeyDown(KeyCode.E);                                   //디펜스
        dUp = Input.GetKeyUp(KeyCode.E);
        //kDown = Input.GetKeyDown(KeyCode.K);
        downParryingSkill = Input.GetKeyDown(KeyCode.Q);
        hpRecover = Input.GetKeyDown(KeyCode.Alpha2);
        if (hpRecover && hpItem>0)
        {
            state.HpUp();
            hpItem--;
            OnPotionChanged(hpItem); // 포션 개수 UI

        }
    }

    void Move()
    {
        Vector3 lookForward = new Vector3(transform.forward.x, 0f, transform.forward.z).normalized;
        Vector3 lookRight = new Vector3(transform.right.x, 0f, transform.right.z).normalized;
        moveVec = (lookForward * vAxis + lookRight * hAxis).normalized;

        Debug.DrawRay(transform.position, moveVec * 10f, Color.red);
        Debug.DrawRay(transform.position, transform.forward * 10f, Color.blue);
        Debug.DrawRay(transform.position, transform.right * 10f, Color.blue);

        //장애물 감지
        /*
        if(Physics.Raycast(transform.position, moveVec, out hit, distanceToObstacle))
        {
            if (!hit.collider.isTrigger)
            {
                moveVec = Vector3.zero;
            }
        }
        */
        if (isJump)
        {
            moveVec = jumpVec;
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Death") || isAttack || anim.GetCurrentAnimatorStateInfo(0).IsName("Roll") || isDefense)
        {
            moveVec = Vector3.zero;
        }
        Stamina();
        // 월드 기준
        anim.SetBool("isWalk", moveVec != Vector3.zero);
        anim.SetBool("isRun", rDown);
        anim.SetFloat("Horizontal", hAxis, 0.5f, Time.deltaTime);
        anim.SetFloat("Vertical", vAxis, 0.5f, Time.deltaTime);
        if (moveVec!=Vector3.zero)
        {
              movements = moveVec * speed * (rDown ? 2.0f : 1.0f) * Time.deltaTime;
              characterController.Move(movements);
              //anim.SetFloat("Horizontal", hAxis, 0.5f, Time.deltaTime);
              //anim.SetFloat("Vertical", vAxis, 0.5f, Time.deltaTime);
        }
        DetectOverlapEnemies();
        MoveSound();
    }


    private void DetectOverlapEnemies()
    {
        Vector3 point1 = transform.position+new Vector3 (0.0f,0.86f,0.0f) + transform.up * capsuleHeight / 2;
        Vector3 point2 = transform.position+ new Vector3(0.0f, 0.86f, 0.0f) - transform.up * capsuleHeight / 2;

        Debug.Log(transform.position);

        Collider[] hits = Physics.OverlapCapsule(point1, point2, capsuleRadius, enemyLayerMask);
        foreach (var hit in hits)
        {
            Vector3 direction;
            float distance;
            if (Physics.ComputePenetration(
                    characterController, transform.position, transform.rotation,
                    hit, hit.transform.position, hit.transform.rotation,
                    out direction, out distance
                ))
            {
                Vector3 displacement = direction * distance*0.4f;
                displacement.y = 0;
                characterController.Move(displacement);
            }
        }

    }
    void MouseRotate()
    {
        // 플레이어의 수평회전 처리
        float hRotation = transform.rotation.eulerAngles.y + (mouseValueX * hSensivity); // 수평 회전 값(y축 회전)
        transform.rotation = Quaternion.Euler(0f, hRotation, 0f);

        // 카메라의 수직회전을 위한 프로퍼티
        VRotation -= (mouseValueY * vSensivity);                                         // 수직 회전 값(x축 회전)
        VRotation = Mathf.Clamp(VRotation, 25f, 70f);
    }

    void Jump()                                                                         //현재 구르기로 사용중
    {
        jumpVec = moveVec;
        if (jDown && !isJump && !isAttack)
        {
            isJump = true;
            anim.SetTrigger("doJump");
            state.DecreaseStamina(5);
            characterSound.PlayCharacterSound("Roll");
            Invoke("JumpOut", 1.0f);
        }
    }

    void JumpOut()
    {
        isJump = false;
    }

    void Stamina()
    {
        float staminaDecreasePerSec = 5;
        if (rDown && moveVec != Vector3.zero)
        {
            if (state.moveStats.stamina < 5)
            {
                rDown = false;
                return;
            }
            //state.moveStats.stamina -= staminaDecreasePerSec * Time.deltaTime;
            state.DecreaseStamina(staminaDecreasePerSec * Time.deltaTime);
        }
        if (!rDown && state.moveStats.stamina < 100)
        {
            //state.moveStats.stamina += staminaDecreasePerSec-2 * Time.deltaTime;
            state.IncreaseStamina((staminaDecreasePerSec - 2) * Time.deltaTime);
        }
    }

    void Defenssing()
    {
        if (dDown&&!isDefenseCool)                                         //처음 눌렀을 때(디펜스 시작 시)
        {
            isDefense = true;
            isDefenseCool = true;
            
            anim.SetBool("Defense", true);
            anim.SetTrigger("doDefense");
            attack_controller.weapon_right.ShieldEffectInstance();
            state.DefUp();
        }

        if(dUp&&isDefense) {                                             //디펜스 끝날 때
            isDefense = false;
            state.basicStats.def -= 100;
            anim.SetBool("Defense", false);
            attack_controller.weapon_right.ShieldEffectOut();
            state.DefDown();
            Invoke("isDefensCoolDown", 30.0f);
        }
    }
    void isDefensCoolDown()
    {
        isDefenseCool = false;
    }

    public void DefensingHit()
    {
        anim.SetTrigger("getDefenseHIt");
    }

    void CharacterAttackSounds(int key)
    {
        if(key == 0)
        {
            characterSound.PlayCharacterSound("Attack");
        }
        else 
        { 
            characterSound.PlayCharacterSound("ComboAttack"); 
        }
        
    }
    void attack1()         
    {
        attack_controller.attack1();
    }

    void attack2()
    {
        attack_controller.attack2();
    }

    void strongAttack()
    {
        if (!attack_controller.strongCoolTime)
        attack_controller.strongAttack();
    }

    void ParryingSkill()
    {
        if (!isParrying && !isAttack&&!ParryingCoolTime)
        {
            isParrying = true;
            attack_controller.parryingAttack();
        }
        
    }

    public void ParryingSkillOut()
    {
        ParryingCoolTime = true;
        isParrying = false;
        attack_controller.weapon_right.parryingAttack = false;
        if (attack_controller.weapon_left != null) attack_controller.weapon_left.parryingAttack = false;
        Invoke("ParryingCoolTimeOut", 3.0f);
    }

    void ParryingCoolTimeOut()
    {
        ParryingCoolTime = false;
    }
    public void isAttackAnimation()             //공격 애니메이션 공용 이벤트 1 (시작 지점)
    {
        isAttack = true;
    }

    public void WeaponUse(int key)                     //공격 애니메이션 공용 이벤트 2  (공격 모션 시작하는 지점)
    {
        attack_controller.weapon_right.Use(key);
    }

    public void WeaponUseLeft(int key)                //공격 애니메이션 공용 이벤트 2_2 
    {
        attack_controller.weapon_left.Use(key);
    }
    public void WeaponAttackOut()               //공격 애니메이션 공용 이벤트 3 (공격 모션 끝나는 지점)
    {
        attack_controller.weapon_right.AttackOut();
    }

    public void WeaponAttackOutLeft()           //공격 애니메이션 공용 이벤트 3_2
    {
        attack_controller.weapon_left.AttackOut();
    }
    public void isAttackAnimationEnd()          //공격 애니메이션 공용 이벤트 4 (종료 지점)
    {
        isAttack = false;
        attack_controller.weapon_right.isHeavyAttack = false;
        if (attack_controller.weapon_left != null) { attack_controller.weapon_left.isHeavyAttack = false; }
    }
   

    void Death()
    {
        if (!isDeath)
        {
            anim.SetTrigger("doDeath");
            isDeath = true;
            characterSound.PlayCharacterSound("Death");
            Invoke("DestroyPlayer", 5.0f);
        }
    }

    void DestroyPlayer()
    {
        PhotonView PV = this.GetComponent<PhotonView>();
        if (PV.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
        //Destroy(gameObject);
    }

    void MoveSound()
    {
        if (!isJump&&!isAttack&&!isDeath)
        {
            if (moveVec != Vector3.zero)
            { characterSound.PlayWalkSound(); }
            else
            { characterSound.StopFootsteps(); }
            characterSound.IsRunning(rDown);
        }
    }
    public void SetVSensivity(float value)
    {
        vSensivity = value;
    }
    public void SetHSensivity(float value)
    {
        hSensivity = value;
    }
    public void Teleport()
    {

    }
    ///// <summary>
    ///// 스탯창 UI 설정
    ///// </summary>
    //void CkeckUI()
    //{
    //    if (kDown)
    //    {
    //    Debug.Log("유아이 호출");
    //        uiStatus.SetUI();
    //    }

    //}
}
