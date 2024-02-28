using UnityEngine;

public class SpawnPortal : MonoBehaviour
{
    public GameObject objectToSpawn; // 생성할 오브젝트를 지정하는 공개 변수
    public float spawnDelay = 5.0f; // 생성까지의 지연 시간을 지정하는 공개 변수

    // Start 메서드는 씬이 시작될 때 호출되는 메서드입니다.
    void Start()
    {
        // Invoke 메서드는 지정된 메서드를 일정 시간 후에 호출하는 함수입니다.
        // 여기서는 "SpawnObject" 메서드를 spawnDelay 시간 후에 호출하도록 설정했습니다.
        Invoke("SpawnObject", spawnDelay);
    }

    // 오브젝트를 생성하는 메서드입니다.
    void SpawnObject()
    {
        // Instantiate 메서드는 지정된 오브젝트의 복제본을 생성하는 함수입니다.
        // 여기서는 objectToSpawn 오브젝트를 복제하여 새 오브젝트를 생성합니다.
        Instantiate(objectToSpawn, transform.position, Quaternion.identity);
    }
}
