using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    [SerializeField] int currentHP = 100;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsBreak())
        {
            Break();
        }
    }

    void OnTriggerEnter(Collider coll)
    {
        Debug.Log("�浹");
        if (coll.tag == "Melee")
        {
            currentHP -= 10;
            Debug.LogWarning("�����浹");
          

        }
    }
    private bool IsBreak()
    {
        return currentHP <= 0;
    }


    private void Break()
    {
        // �ı�
        GameManager.Instance.GameFinish();
        Destroy(gameObject);
    }
}
