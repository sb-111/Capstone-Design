using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GrowthSystem : MonoBehaviour
{
    private int soulCount;

    private PlayerStatus status;
    private int hpLevel;
    private int defLevel;
    private int atkLevel;
    private int dexLevel;
    private int intLevel;
    private Dictionary<int, int> soulPerLevel;

    public delegate void SoulChangedHandler(string count);
    public static event SoulChangedHandler OnSoulChanged; // 발행자의 event가 먼저 초기화 되어있어야 함, static이라 관리 주의
    private void Awake()
    {
        hpLevel = 1;
        defLevel = 1;
        atkLevel = 1;
        dexLevel = 1;
        intLevel = 1;

        soulCount = 500;

        soulPerLevel = new Dictionary<int, int>()
        {
            {1,5}, {2,10}, {3,15}, {4,20}, {5,25},
            {6,30}, {7,35}, {8,40}, {9,45}, {10,50},
        };
        Debug.Log($"Awake: {soulPerLevel.Count}");
    }
    private void Start()
    {
        status = GetComponent<PlayerStatus>();
        Debug.Log("GrowthSystem ID: " + GetInstanceID());
    }
    /// <summary>
    /// Soul 획득처리 및 UI 표시
    /// </summary>
    public void GetSoul(int count)
    {
        soulCount += count;
        OnSoulChanged(soulCount.ToString()); // UI 업데이트 이벤트 발생
        Debug.Log($"소울딕셔너리:{soulPerLevel.Count}" );
    }
    public void OnPressEnhanceHealthBtn()
    {
        Debug.Log("개수:"+ soulPerLevel.Count);
        if(hpLevel < 10 && soulCount >= soulPerLevel[hpLevel])
        {
            soulCount -= soulPerLevel[hpLevel];
            //uiSoul.UpdateUI(soulCount.ToString());
            hpLevel++;
            status.IncreaseHealth();
            OnSoulChanged(soulCount.ToString()); // UI 업데이트 이벤트 발생
        }
    }
    public void OnPressEnhanceDefenseBtn()
    {
        if (defLevel < 10 && soulCount >= soulPerLevel[defLevel])
        {
            soulCount -= soulPerLevel[defLevel];
            //uiSoul.UpdateUI(soulCount.ToString());
            defLevel++;
            status.IncreaseDefense();
            OnSoulChanged(soulCount.ToString()); // UI 업데이트 이벤트 발생
        }
    }
    public void OnPressEnhanceAttackBtn()
    {
        if (atkLevel < 10 && soulCount >= soulPerLevel[atkLevel])
        {
            soulCount -= soulPerLevel[atkLevel];
            //uiSoul.UpdateUI(soulCount.ToString());
            atkLevel++;
            status.IncreaseAttack();
            OnSoulChanged(soulCount.ToString()); // UI 업데이트 이벤트 발생
        }
    }
    public void OnPressEnhanceDexBtn()
    {
        if (dexLevel < 10 && soulCount >= soulPerLevel[dexLevel])
        {
            soulCount -= soulPerLevel[dexLevel];
            //uiSoul.UpdateUI(soulCount.ToString());
            dexLevel++;
            status.IncreaseDex();
            OnSoulChanged(soulCount.ToString()); // UI 업데이트 이벤트 발생
        }
    }
    public void OnPressEnhanceIntBtn()
    {
        if (intLevel < 10 && soulCount >= soulPerLevel[intLevel])
        {
            soulCount -= soulPerLevel[intLevel];
            //uiSoul.UpdateUI(soulCount.ToString());
            intLevel++;
            status.IncreaseInt();
            OnSoulChanged(soulCount.ToString()); // UI 업데이트 이벤트 발생
        }
    }
    public int GetDicCnt()
    {
        return soulPerLevel.Count;
    }
   
}
