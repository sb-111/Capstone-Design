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
    private int speed;
    private float jumpPower;

    Vector3 moveVec;
    Vector3 jumpVec;

    bool rDown;
    bool jDown;
    bool isDeath;
    public bool dDown;
    public bool isJump;
    public bool isDefense;

    //공격
    public float attackDelay = 1.0f;
    bool isAttackReady;
    bool left_attack;                       //좌클릭 공격
    bool right_attack;                      //우클릭 공격
    bool strong_attack;                     //좌+우클릭 공격 합친거
    public bool isAttack = false;                          //공격 중?
    bool canAttack;

    AttackController attack_controller;

    //패링
    bool isParrying;

    //넉백
    public float knockbackForce = 0.5f;
    public float knockbackTime = 0.3f;
    bool isKnockback;

    public Animator anim;
    Rigidbody rigid;
    PlayerStatus state;
    public CameraShake cameraShaking;

    float sensivity = 1f;
    public float VRotation { get;  private set; } // 수직 회전 값

    // Start is called before the first frame update
    private void Awake()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        state = GetComponent<PlayerStatus>();
        attack_controller = GetComponent<AttackController>();
        cameraShaking = Camera.main.GetComponent<CameraShake>();
    }
    void Start()
    {
        speed = state.moveStats.speed;
        //jumpPower = state.moveStats.jumpPower;
    }

    // Update is called once per frame
    void Update()
    {


        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }

        attackDelay += Time.deltaTime;
        isAttackReady = state.combatStats.attack_rate <= attackDelay;
        

        GetInput();
        MouseRotate();
        Move();
      
        Jump();
        if (state.basicStats.hp <= 0) { Death(); }
        hit();
        //if (isAttackReady && !isJump && !isDeath && !isAttack) canAttack = true;
        if (isAttackReady && !isJump && !isDeath) canAttack = true;
        else canAttack = false;
        attack_controll();
        Defenssing(dDown);
        if(!dDown) isDefense = false;
    }
    void attack_controll()
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
    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal"); // x축 이동(-1/1)
        vAxis = Input.GetAxisRaw("Vertical"); // z축 이동(-1/1)
        rDown = Input.GetKey(KeyCode.LeftShift);//leftshift
        jDown = Input.GetKeyDown(KeyCode.Space);//spacebar
        left_attack = Input.GetMouseButtonDown(0);
        right_attack = Input.GetMouseButtonDown(1);
        strong_attack = Input.GetMouseButtonDown(2);

        dDown = Input.GetKey(KeyCode.E); //디펜스
        


        mouseValueX = Input.GetAxis("Mouse X"); // 마우스 수평 회전 값
        mouseValueY = Input.GetAxis("Mouse Y"); // 마우스 수직 회전 값


    }

    void Move()
    {
        Vector3 lookForward = new Vector3(transform.forward.x, 0f, transform.forward.z).normalized;
        Vector3 lookRight = new Vector3(transform.right.x, 0f, transform.right.z).normalized;
        moveVec = (lookForward * vAxis + lookRight * hAxis).normalized;

        //moveVec = (transform.forward * vAxis + transform.right * hAxis).normalized;

        Debug.Log($"forward:{transform.forward}, vAxis:{vAxis}, right:{transform.right}, hAxis:{hAxis}\n" +
            $"transform.forward * vAxis:{transform.forward * vAxis}, transform.right * hAxis:{transform.right * hAxis}");

        Debug.Log($"moveVec : {moveVec}, moveVec의 크기:{moveVec.magnitude}");

        Debug.DrawRay(transform.position, moveVec * 10f, Color.red) ;
        Debug.DrawRay(transform.position, transform.forward * 10f, Color.blue);
        Debug.DrawRay(transform.position, transform.right * 10f, Color.blue);


        if (isJump)
        {
            moveVec = jumpVec;
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Death") || isAttack || anim.GetCurrentAnimatorStateInfo(0).IsName("Roll") || isKnockback || anim.GetCurrentAnimatorStateInfo(0).IsName("Defending"))
        {
            moveVec = Vector3.zero;
        }
        // 월드 기준
        transform.position += moveVec * speed * (rDown ? 2.0f : 1.0f) * Time.deltaTime;
        anim.SetBool("isWalk", moveVec != Vector3.zero);
        anim.SetBool("isRun", rDown);
    }

    void MouseRotate()
    {
        // 플레이어의 수평회전 처리
        float hRotation = transform.rotation.eulerAngles.y + (mouseValueX * sensivity); // 수평 회전 값(y축 회전)
        transform.rotation = Quaternion.Euler(0f, hRotation, 0f);

        // 카메라의 수직회전을 위한 프로퍼티
        VRotation -= (mouseValueY * sensivity); // 수직 회전 값(x축 회전)
        VRotation = Mathf.Clamp(VRotation, 25f, 70f);
    }
    void Jump()
    {
        jumpVec = moveVec;
        if (jDown && !isJump && !isAttack)
        {
            isJump = true;
            anim.SetBool("isJump", true);
            /*if (anim.GetCurrentAnimatorStateInfo(0).IsName("Roll"))
            {
                return;
            }*/

            anim.SetTrigger("doJump");

            Invoke("JumpOut", 1.16f);
        }
    }

    void JumpOut()
    {
        anim.SetBool("isJump", false);
        isJump = false;
    }


    void Defenssing(bool dDwon)
    {
        if(dDown && !isDefense)
        {
            anim.SetBool("Defense", dDown);
        }
        if(!dDown&&isDefense) {

            attack_controller.weapon_right.ShieldEffectOut();
            isDefense = false;
            anim.SetBool("Defense", dDown);
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
        attackDelay = 0;
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

    public void WeaponAttackOut()               //공격 애니메이션 공용 이벤트 3 (공격 모션 끝나는 지점)
    {
        attack_controller.weapon_right.AttackOut();
    }
    public void isAttackAnimationEnd()          //공격 애니메이션 공용 이벤트 4 (종료 지점)
    {
        isAttack = false;
    }
   

    bool CanAttack()
    {
        return isAttackReady && !isJump && !isDeath;
    }
     
    void hit()
    {

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


    public void TakeDamage(int damage, Vector3 enemnyPosition)
    {
       state.TakeDamage(damage, enemnyPosition);
    }
    void OnCollisionEnter(Collision collision)
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

    }
 


    public void Knockback(Vector3 enemyVec)
    {
        isKnockback = true;
        StartCoroutine(OnKnockback(enemyVec));
    }


    IEnumerator OnKnockback(Vector3 enemyVec)
    {
        Debug.Log("넉백");
        float startTime = Time.time;
        
        Vector3 reactVec = (transform.position - enemyVec).normalized;
        reactVec.y += 1.0f;
        rigid.AddForce(reactVec * knockbackForce, ForceMode.Impulse);
        while (Time.time < startTime + knockbackTime)
        {
            yield return null;
        }
        isKnockback = false;
    }

}
