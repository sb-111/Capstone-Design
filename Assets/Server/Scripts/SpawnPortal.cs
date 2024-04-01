using UnityEngine;
using System.Collections;

public class SpawnPortal : MonoBehaviour
{
    public GameObject objectToSpawn;
    public float spawnDelay = 10.0f;
    public Vector3[] spawnPositions;

    /*void Start()
    {
        StartCoroutine(DelayedSpawn(spawnDelay));
    }*/

    public IEnumerator DelayedSpawn(float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnObject();
    }

    public void SpawnObject()
    {
        Vector3 spawnPosition = spawnPositions[Random.Range(0, spawnPositions.Length)];
        Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
    }
}
