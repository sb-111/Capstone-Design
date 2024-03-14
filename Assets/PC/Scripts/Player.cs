using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float hAxis;
    float vAxis;
    public float speed = 2.0f;
    public float jumpPower = 4.0f;

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
    public Weapon weapon_left;
    public Weapon weapon_right;

    //패링
    bool isParrying;

    //넉백
    public float knockbackForce = 5.0f;
    public float knockbackTime = 0.5f;
    bool isKnockback;

    Animator anim;
    Rigidbody rigid;
    PlayerState state;

    // Start is called before the first frame update
    private void Awake()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        state = GetComponent<PlayerState>();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();
        Turn();
        Jump();
        if (state.hp <= 0) { Death(); }
        attack1();
        attack2();
        strongAttack();
        hit();
    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        rDown = Input.GetKey(KeyCode.LeftShift);//leftshift _ 나중에 런으로 바꿔야 함ㅇㅇ
        jDown = Input.GetKeyDown(KeyCode.Space);//spacebar
        left_attack = Input.GetMouseButtonDown(0);
        right_attack = Input.GetMouseButtonDown(1);
        strong_attack  = Input.GetMouseButtonDown(2);
        //if(left_attack && right_attack) {strong_attack = true;}       //이러니까 계속 공격 바복된다. 다시 꺼야 함.
    }

    void Move()
    {

        moveVec = new Vector3(hAxis, 0, vAxis).normalized;
        if(isJump){
             moveVec = jumpVec;
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("death3") || isAttack || anim.GetCurrentAnimatorStateInfo(0).IsName("dodge")|| isKnockback)
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
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            isJump = true;
        }
    }

    void attack1()          //말고 그냥 코루틴으로 바꿀까? 흠
    {

        attackDelay += Time.deltaTime;
        isAttackReady = weapon_left.rate < attackDelay;

        if (left_attack && isAttackReady && !isJump && !isDeath && !isAttack)
        {
            StopCoroutine(coAttack1());
            StartCoroutine(coAttack1());
        }
    }

    IEnumerator coAttack1()         //좌클릭 공격 /이 코루틴 내에서 시작 시간 설정하고 Use에 해당 공격의 무기 트리거 종료 시간을 넘기자.
    {
        isAttack = true;
        anim.SetTrigger("doLattack");
        weapon_left.Use(1.05f);         //두 무기 동시에 쓰는 공격임
        weapon_right.Use(1.05f);
        attackDelay = 0;                //attack딜레이가 공격 시작하자마자 0되는게 맞는가???
        yield return new WaitForSeconds(2.0f);
        attackOut();
    }


    void attack2()
    {
        if (right_attack && isAttackReady && !isJump && !isDeath && !isAttack)
        {
            StopCoroutine(coAttack2());
            StartCoroutine(coAttack2());
        }
    }

    IEnumerator coAttack2()         //좌클릭 공격 /이 코루틴 내에서 시작 시간 설정하고 Use에 해당 공격의 무기 트리거 종료 시간을 넘기자.
    {
        isAttack = true;
        anim.SetTrigger("doRattack");
        weapon_right.Use(0.15f);
        attackDelay = 0;
        yield return new WaitForSeconds(2.0f);
        attackOut();
    }


    void strongAttack()
    {
        if (strong_attack && isAttackReady && !isJump && !isDeath && !isAttack)
        {
            StopCoroutine(coStrongAttack());
            StartCoroutine(coStrongAttack());
        }
    }

    IEnumerator coStrongAttack()
    {
        isAttack = true;
        anim.SetTrigger("doStrongAttack");
        yield return new WaitForSeconds(0.6f);          //생각한 것 보다 0.01초 차이로 빠르게 켜지거나 늦게 켜짐
        weapon_left.Use(0.3f);
        yield return new WaitForSeconds(0.26f);         //0.14s late
        weapon_right.Use(0.6f);
        yield return new WaitForSeconds(0.85f);         //0.008s late
        weapon_right.Use(0.31f);
        weapon_left.Use(0.31f);
        yield return new WaitForSeconds(0.63f);         //0.001s fast
        weapon_left.Use(0.35f);
        attackDelay = 0;
        yield return new WaitForSeconds(0.6f);
        attackOut();
    }

        void attackOut()
    {
        isAttack = false;
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
