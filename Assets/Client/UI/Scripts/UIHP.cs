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
        status.OnStaminaBarChanged += UpdateStamina;
    }

    public void UpdateHp(int currentHP, int maxHP)
    {
        hpBar.fillAmount = (float)currentHP / maxHP;
    }
    public void UpdateStamina(float currentStamina, float maxStamina)
    {
        staminaBar.fillAmount = (float)currentStamina / maxStamina;
    }
    private void OnDestroy()
    {
        status.OnHPBarChanged -= UpdateHp; 
        status.OnStaminaBarChanged -= UpdateStamina;
    }
}
