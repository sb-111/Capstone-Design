using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMove : MonoBehaviour
{
    float xAxis;
    float zAxis;
    public float moveSpeed;
    Vector3 moveVec;

    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        InputKey();
        Move();
    }

    private void Move()
    {
        moveVec = new Vector3(xAxis, 0, zAxis).normalized;

        transform.position += moveVec * moveSpeed * Time.deltaTime;
        animator.SetBool("Walk", moveVec != Vector3.zero);
    }

    void InputKey()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
        zAxis = Input.GetAxisRaw("Vertical");
    }
}
