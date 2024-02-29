using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hit_test : MonoBehaviour
{
    public int hp = 1000;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Melee")
        {
            hp = hp-other.GetComponent<Weapon>().damage;
        }
    }

}
