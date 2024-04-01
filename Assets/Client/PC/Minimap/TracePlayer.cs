using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracePlayer : MonoBehaviour
{
    [SerializeField]
    private bool x, y, z;           //target의 좌표 카피, target이 fasle면 그대로 유지 x,z만 사용할 것 
    [SerializeField]
    private Transform target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!target) return;

        transform.position = new Vector3(
            (x ? target.position.x : transform.position.x),
            (y ? target.position.y : transform.position.y),
            (z ? target.position.z : transform.position.z));

    }
}
