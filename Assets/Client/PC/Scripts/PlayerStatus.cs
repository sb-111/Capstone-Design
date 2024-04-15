using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStatus : MonoBehaviour
{
    
    public BasicStats basicStats;
    public MoveStats moveStats;
    public CombatStats combatStats;

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
        public int stamina = 100;
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
   
}
