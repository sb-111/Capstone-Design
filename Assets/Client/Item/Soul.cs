using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soul : MonoBehaviour
{
    private Monster.MonsterType monsterType;

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
            GrowthSystem growthSystem = other.gameObject.GetComponent<GrowthSystem>();
            int soulAmount = GetSoulAmount(monsterType); // 몬스터 타입에따른 소울 개수 반환
            growthSystem.GetSoul(soulAmount); // 플레이어 획득 처리
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// 몬스터 종류에 따른 반환할 소울 개수 설정
    /// </summary>
    /// <param name="type">몬스터 타입</param>
    /// <returns>소울 개수</returns>
    private int GetSoulAmount(Monster.MonsterType type)
    {
        int amount = 0;
        switch(type)
        {
            case Monster.MonsterType.Cyclops:
                amount = 25;
                break;
            case Monster.MonsterType.Goblin:
                amount = 10;
                break;
            case Monster.MonsterType.Hobgoblin:
                amount = 20;
                break;
            case Monster.MonsterType.Kobold: 
                amount = 10;
                break;  
            case Monster.MonsterType.Troll: 
                amount = 25;
                break;
        }
        return amount;
    }
    /// <summary>
    /// Setter: monsterType 
    /// </summary>
    /// <param name="monsterType"></param>
    public void SetMonsterType(Monster.MonsterType monsterType)
    {
        this.monsterType = monsterType;
    }
}
