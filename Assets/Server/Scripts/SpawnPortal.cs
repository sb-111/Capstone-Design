using UnityEngine;

public class SpawnPortal : MonoBehaviour
{
    public GameObject objectToSpawn;
    public float spawnDelay =10.0f;
    public Vector3[] spawnPositions;

    void Start()
    {
        //Invoke("SpawnObject", spawnDelay);
    }

    public void SpawnObject()
    {
        Vector3 spawnPosition = spawnPositions[Random.Range(0, spawnPositions.Length)];
        Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
    }
}
