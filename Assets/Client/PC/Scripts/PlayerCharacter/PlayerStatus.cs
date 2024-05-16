using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class PlayerStatus : MonoBehaviour
{
    
    public BasicStats basicStats;
    public MoveStats moveStats;
    public CombatStats combatStats;

    [SerializeField] private UIStatus uiStatus;
    [SerializeField] private UnityEvent OnHPChanged;
    //const float MaxHealth = 2000f;
    //const float MaxAttack = 500f;
    //const float MaxDefense = 300f;
    //const float MaxDex = 300f;
    //const float MaxInt = 300f;

    [System.Serializable]
    public class BasicStats
    {
        public int hp = 100;
        public int maxhp = 100;
        public int def = 10;
        public int atk = 30;
        public int dex = 30;
        public int intell = 30;
        public int luk;
    }

    [System.Serializable]
    public class MoveStats
    {
        public float stamina = 100;
        public int speed = 10;
        //public float jumpPower = 7.0f;
    }

    [System.Serializable]
    public class CombatStats
    {
        public float constant_def = 100.0f;
        public float attack_rate=0.2f;
        public float critical_rate;
        public float critical_damage;
        public float cc_resistance;
    }



    Rigidbody rigid;
    Player player;

    private void Awake()
    {
        player = GetComponent<Player>();  
        rigid = GetComponent<Rigidbody>();
    }
    public void IncreaseHealth()
    {
        basicStats.maxhp += 100;
        //uiStatus.
    }
    public void IncreaseDefense()
    {
        basicStats.def += 10;
    }
    public void IncreaseAttack() 
    {
        basicStats.atk += 30;
    }
    public void IncreaseDex()
    {
        basicStats.dex += 30;
    }
    public void IncreaseInt()
    {
        basicStats.intell += 30;
    }
}
