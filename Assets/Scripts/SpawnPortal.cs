using UnityEngine;

public class SpawnPortal : MonoBehaviour
{
    public GameObject objectToSpawn;
    public float spawnDelay =10.0f;

    void Start()
    {
        //Invoke("SpawnObject", spawnDelay);
    }

    // 오브젝트를 생성하는 메서드입니다.
    public void SpawnObject()
    {
        Instantiate(objectToSpawn, transform.position, Quaternion.identity);
    }
}