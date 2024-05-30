using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
public class UIHP : MonoBehaviourPun
{
    [SerializeField] private PlayerStatus status;
    [SerializeField] private Image hpBar;
    [SerializeField] private Image staminaBar;
    [SerializeField] private TextMeshProUGUI potionText;
    private Player player;
    // Start is called before the first frame update
    private void Awake()
    {
        player = GetComponentInParent<Player>();
        if (!GetComponent<PhotonView>().IsMine)
        {
            Destroy(gameObject);
            return;
        }

        status.OnHPBarChanged += UpdateHp;
        status.OnStaminaBarChanged += UpdateStamina;
        player.OnPotionChanged += UpdatePotionCount;
    }

    public void UpdateHp(int currentHP, int maxHP)
    {
        hpBar.fillAmount = (float)currentHP / maxHP;
    }
    public void UpdateStamina(float currentStamina, float maxStamina)
    {
        staminaBar.fillAmount = (float)currentStamina / maxStamina;
    }
    public void UpdatePotionCount(int count)
    {
        potionText.text = count.ToString();
    }
    private void OnDestroy()
    {
        //status.OnHPBarChanged -= UpdateHp; 
        //status.OnStaminaBarChanged -= UpdateStamina;
        //player.OnPotionChanged -= UpdatePotionCount;

        if (status != null)
        {
            status.OnHPBarChanged -= UpdateHp;
            status.OnStaminaBarChanged -= UpdateStamina;
        }

        if (player != null)
        {
            player.OnPotionChanged -= UpdatePotionCount;
        }
    }
}
