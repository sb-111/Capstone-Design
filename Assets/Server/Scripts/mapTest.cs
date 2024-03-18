using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class mapTest : MonoBehaviour
{
    // Start is called before the first frame update
    NavMeshAgent agent;

    [SerializeField]
    Transform target;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            agent.SetDestination(target.position);
            //목적지 알려주는 함수
        }

    }
}
