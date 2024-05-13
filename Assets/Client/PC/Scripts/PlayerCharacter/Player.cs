using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Player : MonoBehaviourPun
{
    float hAxis;
    float vAxis;
    float mouseValueX;
    float mouseValueY;
    int speed;
    float sensivity = 1f;
    Vector3 moveVec;
    Vector3 jumpVec;
    bool isDefenseCool=false;

    bool rDown;                                             //달리기 키
    bool jDown;                                             //구르기 키
    bool isDeath;                                           //죽는 중인가?
    bool kDown;                                             // 스탯창 버튼 눌렀는가
    [HideInInspector]
    public bool isJump;                                     //구르는 중인가?
    [HideInInspector]
    public bool dDown;                                      //방어 키
    bool dUp;
    [HideInInspector]
    public bool isDefense;                                  //방어 중인가?
    [HideInInspector]
    public bool isCC = false;                               //CC 상태인가?


    //공격
    bool left_attack;                       //좌클릭 공격
    bool right_attack;                      //우클릭 공격
    bool strong_attack;                     //좌+우클릭 공격 합친거
    public bool isAttack = false;           //공격 중?
    bool canAttack;

    float soulCount = 0;
    [SerializeField] private UISoul uiSoul;
    [SerializeField] private UIStatus uiStatus;

    [HideInInspector] public AttackController attack_controller;
    public Animator anim { get; private set; }
    public Rigidbody rigid { get; private set; }
    [HideInInspector] public PlayerStatus state;

    public float VRotation { get;  private set; } // 수직 회전 값

    // Start is called before the first frame update
    private void Awake()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        state = GetComponent<PlayerStatus>();
        attack_controller = GetComponent<AttackController>();      
    }
    void Start()
    {
        speed = state.moveStats.speed;
    }

    // Update is called once per frame
    void Update()
    {

        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }

        
        if (!isJump && !isDeath) canAttack = true;
        else canAttack = false;
        
        if (state.basicStats.hp <= 0) { Death(); }

        GetInput();
        MouseRotate();
        //임시 임시 임시
        //Turn();
        //임시 임시 임시
        Move();
        Jump();
        attack_controll();
        Defenssing();
        CkeckUI();
    }
    void attack_controll()                              //공격 입력 관리
    {
        if (canAttack)
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
        kDown = Input.GetKeyDown(KeyCode.K);
    }

    void Move()
    {
        Vector3 lookForward = new Vector3(transform.forward.x, 0f, transform.forward.z).normalized;
        Vector3 lookRight = new Vector3(transform.right.x, 0f, transform.right.z).normalized;
        moveVec = (lookForward * vAxis + lookRight * hAxis).normalized;

        //임시 임시 임시 + 위 주석 해제해서 사용해야 함
        //moveVec = new Vector3(hAxis, 0, vAxis).normalized;
        //임시 임시 임시
        Debug.DrawRay(transform.position, moveVec * 10f, Color.red) ;
        Debug.DrawRay(transform.position, transform.forward * 10f, Color.blue);
        Debug.DrawRay(transform.position, transform.right * 10f, Color.blue);
        

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
        transform.position += moveVec * speed * (rDown ? 2.0f : 1.0f) * Time.deltaTime;
        anim.SetBool("isWalk", moveVec != Vector3.zero);
        anim.SetBool("isRun", rDown);
    }

    //void Turn()
    //{
    //    transform.LookAt(transform.position+moveVec);
    //}

    void MouseRotate()
    {
        // 플레이어의 수평회전 처리
        float hRotation = transform.rotation.eulerAngles.y + (mouseValueX * sensivity); // 수평 회전 값(y축 회전)
        transform.rotation = Quaternion.Euler(0f, hRotation, 0f);

        // 카메라의 수직회전을 위한 프로퍼티
        VRotation -= (mouseValueY * sensivity);                                         // 수직 회전 값(x축 회전)
        VRotation = Mathf.Clamp(VRotation, 25f, 70f);
    }

    void Jump()                                                                         //현재 구르기로 사용중
    {
        jumpVec = moveVec;
        if (jDown && !isJump && !isAttack)
        {
            isJump = true;
            anim.SetTrigger("doJump");
            Invoke("JumpOut", 1.16f);
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
            state.moveStats.stamina -= staminaDecreasePerSec * Time.deltaTime;
        }
        if (!rDown && state.moveStats.stamina < 100)
        {
            state.moveStats.stamina += staminaDecreasePerSec-2 * Time.deltaTime;
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
        }

        if(dUp&&isDefense) {                                             //디펜스 끝날 때
            isDefense = false;
            anim.SetBool("Defense", false);
            attack_controller.weapon_right.ShieldEffectOut();
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
        attack_controller.strongAttack();
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
    }
   

    void Death()
    {
        if (!isDeath)
        {
            anim.SetTrigger("doDeath");
            isDeath = true;

            Invoke("DestroyPlayer", 5.0f);
        }
    }

    void DestroyPlayer()
    {
        Destroy(gameObject);
    }
    /// <summary>
    /// Soul 획득처리 및 UI 표시
    /// </summary>
    public void GetSoul()
    {
        soulCount++;
        uiSoul.UpdateUI(soulCount.ToString());
    }
    /// <summary>
    /// 스탯창 UI 설정
    /// </summary>
    void CkeckUI()
    {
        if (kDown)
        {
            uiStatus.SetUI();
        }
        
    }
}
