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

    bool rDown;                                             //달리기 키
    bool jDown;                                             //구르기 키
    bool isDeath;                                           //죽는 중인가?
    [HideInInspector]
    public bool isJump;                                     //구르는 중인가?
    [HideInInspector]
    public bool dDown;                                      //방어 키
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
        /*if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }*/
        
        if (!isJump && !isDeath) canAttack = true;
        else canAttack = false;
        if (!dDown) isDefense = false;
        if (state.basicStats.hp <= 0) { Death(); }

        GetInput();
        MouseRotate();
        //임시 임시 임시
        Turn();
        //임시 임시 임시
        Move();
        Jump();
        attack_controll();
        Defenssing(dDown);
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
        dDown = Input.GetKey(KeyCode.E);                                   //디펜스
    }

    void Move()
    {
        Vector3 lookForward = new Vector3(transform.forward.x, 0f, transform.forward.z).normalized;
        Vector3 lookRight = new Vector3(transform.right.x, 0f, transform.right.z).normalized;
        //moveVec = (lookForward * vAxis + lookRight * hAxis).normalized;

        //임시 임시 임시 + 위 주석 해제해서 사용해야 함
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;
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
        // 월드 기준
        transform.position += moveVec * speed * (rDown ? 2.0f : 1.0f) * Time.deltaTime;
        anim.SetBool("isWalk", moveVec != Vector3.zero);
        anim.SetBool("isRun", rDown);
    }

    void Turn()
    {
        transform.LookAt(transform.position+moveVec);
    }

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
            //anim.SetBool("isJump", true);                                             //삭제 예정
            anim.SetTrigger("doJump");
            Invoke("JumpOut", 1.16f);
        }
    }

    void JumpOut()
    {
        //anim.SetBool("isJump", false);                                                //삭제 예정
        isJump = false;
    }


    void Defenssing(bool dDwon)
    {
        if(dDown && !isDefense)
        {
            isDefense = true;
            anim.SetBool("Defense", dDown);
            attack_controller.weapon_right.ShieldEffectInstance();
        }
        if(!dDown&&isDefense) {

            isDefense = false;
            anim.SetBool("Defense", dDown);
            attack_controller.weapon_right.ShieldEffectOut();
            
        }
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

    public void attackOut()
    {
        //isAttack = false;
    }

    public void isAttackAnimation()             //공격 애니메이션 공용 이벤트 1 (시작 지점)
    {
        isAttack = true;
    }

    public void WeaponUse()                     //공격 애니메이션 공용 이벤트 2  (공격 모션 시작하는 지점)
    {
        attack_controller.weapon_right.Use();
    }

    public void WeaponUseLeft()                //공격 애니메이션 공용 이벤트 2_2 
    {
        attack_controller.weapon_left.Use();
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
}
