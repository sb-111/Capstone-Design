using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coltest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Ãæµ¹"+other.tag);

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
