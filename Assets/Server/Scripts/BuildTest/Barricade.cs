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
    public void TakeDamage(int damage, Vector3 enmenyPosition)
    {
        currentHP -= damage;

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
        // ÆÄ±«
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
