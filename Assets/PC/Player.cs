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

    bool rDown;
    bool jDown;
    bool isDeath;
    public bool isJump;

    //공격
    float attackDelay = 0.0f;
    bool isAttackReady;
    bool left_attack;
    bool right_attack;
    bool isAttack;
    public Weapon weapon_left;
    public Weapon weapon_right;

    Animator anim;
    Rigidbody rigid;

    public int hp = 100;
    public int def = 10;
    public int atk = 30;
    private int currentHp;
    // Start is called before the first frame update
    private void Awake()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
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
        if (hp <= 0) { Death(); }
        attack1();
        attack2();
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
    }

    void Move()
    {

        moveVec = new Vector3(hAxis, 0, vAxis).normalized;
        ///if(isDodge){
        ///     moveVec = DodgeVec;
        ///}

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("jump") || anim.GetCurrentAnimatorStateInfo(0).IsName("death3") || isAttack)
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
        if (jDown && moveVec == Vector3.zero && !isJump && !isAttack)
        {
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            isJump = true;
        }
    }

    void attack1()
    {

        attackDelay += Time.deltaTime;
        isAttackReady = weapon_left.rate < attackDelay;

        if (left_attack && isAttackReady && !isJump && !isDeath && !isAttack)
        {
            weapon_left.Use();
            anim.SetTrigger("doLattack");
            attackDelay = 0;
            isAttack = true;

            Invoke("attackOut", 2.0f);
        }
    }

    void attack2()
    {
 
        if (right_attack && isAttackReady && !isJump && !isDeath && !isAttack)
        {
            weapon_right.Use();
            anim.SetTrigger("doRattack");
            attackDelay = 0;
            isAttack = true;

            Invoke("attackOut", 1.5f);
        }
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
            isJump = false;

        }
    }
}
