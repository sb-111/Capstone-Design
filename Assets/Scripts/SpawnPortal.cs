using UnityEngine;

public class SpawnPortal : MonoBehaviour
{
    public GameObject objectToSpawn;
    public float spawnDelay =10.0f;

    void Start()
    {
        //Invoke("SpawnObject", spawnDelay);
    }

    public void SpawnObject()
    {
        Instantiate(objectToSpawn, transform.position, Quaternion.identity);
    }
}