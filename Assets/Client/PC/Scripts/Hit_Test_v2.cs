using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit_Test_v2 : MonoBehaviour
{
    public int currentHP = 10000;
    public int def = 50;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        //Debug.Log("데미지를 " + damage +" 만큼 입었습니다.");
    }
}
