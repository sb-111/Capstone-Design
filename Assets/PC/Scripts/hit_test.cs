using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hit_test : MonoBehaviour
{
    public int hp = 1000;
    public bool pDown;

    public BoxCollider ParryingPoint;
    // Start is called before the first frame update
    void Awake()
    {
           
    }

    // Update is called once per frame
    void Update()
    {
        pDown = Input.GetKeyDown(KeyCode.P);
        if (pDown) { Parrying(); }
       
    }

   void Parrying()
    {       ParryingPoint.enabled = true;
        CancelInvoke("ParryingOut");
            Invoke("ParryingOut",3.0f);
    }

    void ParryingOut()
    {
        ParryingPoint.enabled= false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Melee")
        {
            hp = hp-(other.GetComponent<Weapon>().damage);
        }
    }

}
