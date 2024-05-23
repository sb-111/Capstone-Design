using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCounter : MonoBehaviour
{
    [SerializeField]
    public int monType; //0: cyclops 1: goblin; 2:hobgoblin 3:kobold 4:Troll
    [SerializeField]
    public int monMax;
    [SerializeField]
    private float detectionRadius = 5f;
    private int monNum;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    public int controlMonNum()
    {
        int a = 0;
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Enemy"))
            {
                a++; // 대상 태그를 가진 물체이면 개수 증가
            }
        }

        monNum = a;
        return monNum;

    }
    // Update is called once per frame
    void Update()
    {
       
    }
}
