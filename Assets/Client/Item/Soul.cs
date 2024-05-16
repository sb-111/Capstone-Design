using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soul : MonoBehaviour
{
    //[SerializeField] private float speed = 3.0f;

    [SerializeField]private float distance = 0.5f;
    [SerializeField]private float frequency = 5.0f;
    private float originY;
    // Start is called before the first frame update
    void Start()
    {
        originY = transform.position.y;      
    }

    // Update is called once per frame
    void Update()
    {
        float y = distance * Mathf.Sin(2 * Mathf.PI * frequency * Time.time) ;
        transform.position = new Vector3(transform.position.x, originY + y, transform.position.z);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player player = other.gameObject.GetComponent<Player>();
            player.GetSoul();
            Destroy(gameObject);
        }
    }
}
