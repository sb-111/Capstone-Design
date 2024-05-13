using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBullet : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float bulletSpeed = 1000.0f;
    private Transform thisTransform;
    public int weapon_damage = 30;
    // Use this for initialization
    void Start()
    {
        thisTransform = GetComponent<Transform>();
        FireBullet();
        Invoke("DestoryBullet", 5f);
    }

    void FireBullet()
    {
        GetComponent<Rigidbody>().AddForce(thisTransform.forward * bulletSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameObject player = other.gameObject;
            CombatStatusManager combatStatus = player.GetComponent<CombatStatusManager>();
            combatStatus.TakeDamage((20 + weapon_damage));
            ObjectPool.ReturnObject(this);

        }
       

    
    }
    // Update is called once per frame
    void Update()
    {

    }
    private void DestoryBullet()
    {
        ObjectPool.ReturnObject(this);
    }
}
