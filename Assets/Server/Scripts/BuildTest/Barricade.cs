using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricade : MonoBehaviour
{
    [SerializeField] int currentHP = 100;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if (IsBreak())
        {
            Break();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.LogWarning("충돌");
        if (other.tag == "Melee")
        {
            currentHP -= 10;
            Debug.LogWarning("무기충돌");

        }

    }
    public void TakeDamage(int damage, Vector3 enmenyPosition)
    {
        currentHP -= damage;
        Debug.LogWarning("데미지" + damage);
        if (IsBreak())
        {
            Break();
        }
    }
    private bool IsBreak()
    {
        return currentHP <= 0;
    }

 
    private void Break()
    {
        // 파괴
        Destroy(gameObject);
    }
    // Update is called once per frame
   
}
