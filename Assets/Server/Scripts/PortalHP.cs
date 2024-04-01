using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalHP : MonoBehaviour
{
    [Header("포탈 설정")]
    public int maxHP;
    public int currentHP;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        if (currentHP <= 0)
        {
            GameOver();
        }
    }


    private void GameOver()
    {
        // 게임 종료
       
    }
}
