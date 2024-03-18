using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
   
    private void Awake()
    {
        offset = new Vector3(0.0f, 1.2f, -2.49f);
    }
    void Update()
    {
        transform.position = target.position + offset;
        //transform.rotation=target.rotation;
    }
}
