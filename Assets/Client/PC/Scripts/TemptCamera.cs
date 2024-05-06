using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemptCamera : MonoBehaviour
{
    public Transform mainCamera;
    public Transform target;
    public Vector3 OriginPos = new Vector3(0.0f, 15.0f, -10.0f);
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        mainCamera.transform.position = target.position + OriginPos;
    }
}
