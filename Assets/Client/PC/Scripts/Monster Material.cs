using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMaterial : MonoBehaviour
{
    Monster monster;
    [SerializeField] Material mat_30p;
    [SerializeField] Material mat_70p;
    Renderer renderer;
    // Start is called before the first frame update
    void Start()
    {
        monster = GetComponentInParent<Monster>();
        renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    public void ChangeMat(float hp)
    {
        if (hp <= 0.7 && hp > 0.3)
        {
            renderer.material = mat_70p;
        }
        else if (hp <= 0.3)
        {
            renderer.material = mat_30p;
        }
    }
}
