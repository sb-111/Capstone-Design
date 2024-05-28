using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHP : MonoBehaviour
{
    [SerializeField] private PlayerStatus status;
    [SerializeField] private Image hpBar;
    [SerializeField] private Image staminaBar;
    // Start is called before the first frame update
    private void Awake()
    {
        status.OnHPBarChanged += UpdateHp;
    }

    public void UpdateHp(int currentHP, int maxHP)
    {
        Debug.Log($"hpBar 업데이트{currentHP}");
        hpBar.fillAmount = currentHP / maxHP;
    }
    public void UpdateStamina(int currentStamina, int maxStamina)
    {

    }
    private void OnDestroy()
    {
        status.OnHPBarChanged -= UpdateHp; 
    }
}
