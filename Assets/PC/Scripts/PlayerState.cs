using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public int hp = 100;
    public int def = 10;
    public int atk = 30;

    Rigidbody rigid;
    Player player;

    private void Awake()
    {
        player = GetComponent<Player>();  
        rigid = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Knockback") // 예시
        {
            //hp--=Enemy.~~ 상대 몬스터의 공격력기준으로 hp감소
            Vector3 attackEmemyVec = other.transform.position;
            player.Knockback(attackEmemyVec);

        }
    }
  
}
