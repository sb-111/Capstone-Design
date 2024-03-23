using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Player : MonoBehaviourPun
{
    float hAxis;
    float vAxis;
    private int speed;
    private float jumpPower;

    Vector3 moveVec;
    Vector3 jumpVec;

    bool rDown;
    bool jDown;
    bool isDeath;
    public bool isJump;

    //공격
    float attackDelay = 0.0f;
    bool isAttackReady;
    bool left_attack;                       //좌클릭 공격
    bool right_attack;                      //우클릭 공격
    bool strong_attack;                     //좌+우클릭 공격 합친거
    bool isAttack;                          //공격 중?
    bool canAttack;

    AttackController attack_controller;

    //패링
    bool isParrying;

    //넉백
    public float knockbackForce = 5.0f;
    public float knockbackTime = 0.5f;
    bool isKnockback;

    Animator anim;
    Rigidbody rigid;
    PlayerStatus state;
    

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
        jumpPower = state.moveStats.jumpPower;
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
        Move();
        Turn();
        Jump();
        if (state.basicStats.hp <= 0) { Death(); }
        hit();
        if (isAttackReady && !isJump && !isDeath && !isAttack) canAttack = true;
        else canAttack = false;
        attack_controll();
    }
    void attack_controll()
    {
        if (canAttack)
        {
            if (left_attack)
            {
                isAttack = true;
                attack1();
            }
            else if (right_attack)
            {
                isAttack = true;
                attack2();
            }
            else if (strong_attack)
            {
                isAttack = true;
                strongAttack();
            }
        }
    }
    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        rDown = Input.GetKey(KeyCode.LeftShift);//leftshift _ 나중에 런으로 바꿔야 함ㅇㅇ
        jDown = Input.GetKeyDown(KeyCode.Space);//spacebar
        left_attack = Input.GetMouseButtonDown(0);
        right_attack = Input.GetMouseButtonDown(1);
        strong_attack = Input.GetMouseButtonDown(2);
        //if(left_attack && right_attack) {strong_attack = true;}       //이러니까 계속 공격 바복된다. 다시 꺼야 함.
    }

    void Move()
    {

        moveVec = new Vector3(hAxis, 0, vAxis).normalized;
        if (isJump)
        {
            moveVec = jumpVec;
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("death3") || isAttack || anim.GetCurrentAnimatorStateInfo(0).IsName("dodge") || isKnockback)
        {
            moveVec = Vector3.zero;
        }

        transform.position += moveVec * speed * (rDown ? 2.0f : 1.0f) * Time.deltaTime;

        anim.SetBool("isWalk", moveVec != Vector3.zero);
        anim.SetBool("isRun", rDown);
    }

    void Turn()
    {
        transform.LookAt(transform.position + moveVec);
    }

    void Jump()
    {
        jumpVec = moveVec;
        if (jDown && !isJump && !isAttack)
        {
            isJump = true;
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");

            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
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
        attack_controller.strongAttack();
    }

    public void attackOut()
    {
        attackDelay = 0;
        isAttack = false;
    }

    bool CanAttack()
    {
        return isAttackReady && !isJump && !isDeath && !isAttack;
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


    public void TakeDamage(int damage)
    {
       state.basicStats.hp-= damage;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            anim.SetBool("isJump", false);
            StopCoroutine(IsJumpFalse());
            StartCoroutine(IsJumpFalse());
        }
    }

    private void OnTriggerEnter(Collider other)
    {

    }
    public void Parrying()
    {
        anim.SetTrigger("doDodge");
    }


    public void Knockback(Vector3 enemyVec)
    {
        isKnockback = true;
        StartCoroutine(OnKnockback(enemyVec));
    }

    IEnumerator IsJumpFalse()
    {
        yield return new WaitForSeconds(0.7f);
        isJump = false;
    }

    IEnumerator OnKnockback(Vector3 enemyVec)
    {
        float startTime = Time.time;
        Vector3 reactVec = (transform.position - enemyVec).normalized;
        Debug.Log(reactVec);
        rigid.AddForce(reactVec * knockbackForce, ForceMode.Impulse);
        while (Time.time < startTime + knockbackTime)
        {
            yield return null;
        }
        isKnockback = false;
    }

}
