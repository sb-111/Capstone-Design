using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UISoul : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI soulText;
    void Start()
    {
        //soulText.text = "500";
        GrowthSystem.OnSoulChanged += UpdateUI; // 이벤트 구독
    }
    /// <summary>
    /// 소울의 양 Update
    /// </summary>
    /// <param name="quantity">표기할 소울의 양</param>
    public void UpdateUI(string quantity)
    {
        Debug.Log("UpdateUI called with quantity: " + quantity);
        soulText.text = quantity;
    }
    private void OnDestroy()
    {
        GrowthSystem.OnSoulChanged -= UpdateUI; // 이벤트 구독 해제
    }
}
