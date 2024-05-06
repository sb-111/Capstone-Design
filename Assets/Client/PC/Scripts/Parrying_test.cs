using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parrying_test : MonoBehaviour
{
    // Start is called before the first frame update

    public bool pDown;

    public BoxCollider ParryingPoint;
    // Start is called before the first frame update
    void Awake()
    {
        ParryingPoint = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        pDown = Input.GetKeyDown(KeyCode.P);
        Parrying();
    }

    void Parrying()
    {
        if (pDown)
        {
            ParryingPoint.enabled = true;

            Invoke("ParryingOut", 3.0f);
        }
    }

    void ParryingOut()
    {
        ParryingPoint.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Melee")
        {
            Debug.Log("Parrying");
        }
    }
}
