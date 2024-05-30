using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class UIStatus : MonoBehaviourPun
{
    private Player player;
    [SerializeField] private GrowthSystem growthSystem; 
    [SerializeField] private PlayerStatus playerStatus; // 이벤트 발행자

    [SerializeField] private GameObject statBar;
    [SerializeField] private GameObject optionBar;
    [SerializeField] private Button hpEnhanceButton;
    [SerializeField] private Button atkEnhanceButton;
    [SerializeField] private Button defEnhanceButton;
    [SerializeField] private Button dexEnhanceButton;
    [SerializeField] private Button intEnhanceButton;
    [SerializeField] private Button exitButton;

    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI atkText;
    [SerializeField] private TextMeshProUGUI defText;
    [SerializeField] private TextMeshProUGUI dexText;
    [SerializeField] private TextMeshProUGUI intText;
    [SerializeField] private TextMeshProUGUI soulText;
    private void Awake()
    {
     
        if (!GetComponent<PhotonView>().IsMine)
        {
            hpEnhanceButton.onClick.RemoveAllListeners();
            atkEnhanceButton.onClick.RemoveAllListeners();
            defEnhanceButton.onClick.RemoveAllListeners();
            dexEnhanceButton.onClick.RemoveAllListeners();
            intEnhanceButton.onClick.RemoveAllListeners();
            exitButton.onClick.RemoveAllListeners();

            // 이벤트 해제
            playerStatus.OnMaxHPStatChanged -= UpdateMaxHP;
            playerStatus.OnAtkStatChanged -= UpdateAttack;
            playerStatus.OnDefStatChanged -= UpdateDefense;
            playerStatus.OnDexStatChanged -= UpdateDex;
            playerStatus.OnIntStatChanged -= UpdateInt;

            GrowthSystem.OnSoulChanged -= UpdateSoul;
            Destroy(gameObject);
        }
        // 버튼의 모든 리스너 삭제
        hpEnhanceButton.onClick.RemoveAllListeners();
        atkEnhanceButton.onClick.RemoveAllListeners();
        defEnhanceButton.onClick.RemoveAllListeners();
        dexEnhanceButton.onClick.RemoveAllListeners();
        intEnhanceButton.onClick.RemoveAllListeners();
        exitButton.onClick.RemoveAllListeners();

        // 인스턴스 ID 출력
        Debug.Log("UIStatus에서 참조한 growthSystem ID: " + growthSystem.GetInstanceID());

        // 버튼에 리스너 등록
        hpEnhanceButton.onClick.AddListener(growthSystem.OnPressEnhanceHealthBtn);
        atkEnhanceButton.onClick.AddListener(growthSystem.OnPressEnhanceAttackBtn);
        defEnhanceButton.onClick.AddListener(growthSystem.OnPressEnhanceDefenseBtn);
        dexEnhanceButton.onClick.AddListener(growthSystem.OnPressEnhanceDexBtn);
        intEnhanceButton.onClick.AddListener(growthSystem.OnPressEnhanceIntBtn);
        exitButton.onClick.AddListener(OnPressExitBtn);

        // 이벤트 등록(스탯)
        playerStatus.OnMaxHPStatChanged += UpdateMaxHP;
        playerStatus.OnAtkStatChanged += UpdateAttack;
        playerStatus.OnDefStatChanged += UpdateDefense;
        playerStatus.OnDexStatChanged += UpdateDex;
        playerStatus.OnIntStatChanged += UpdateInt;

        // 이벤트 등록(소울 개수)
        GrowthSystem.OnSoulChanged += UpdateSoul;

        player = GetComponentInParent<Player>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K) && !optionBar.activeSelf)
        {
            statBar.SetActive(!statBar.activeSelf);
            player.CanReceiveInput = !statBar.activeSelf;
        }
    }
    private void OnDestroy()
    {
        // 버튼 리스너 해제
        hpEnhanceButton.onClick.RemoveAllListeners();
        atkEnhanceButton.onClick.RemoveAllListeners();
        defEnhanceButton.onClick.RemoveAllListeners();
        dexEnhanceButton.onClick.RemoveAllListeners();
        intEnhanceButton.onClick.RemoveAllListeners();
        exitButton.onClick.RemoveAllListeners();

        // 이벤트 해제
        playerStatus.OnMaxHPStatChanged -= UpdateMaxHP;
        playerStatus.OnAtkStatChanged -= UpdateAttack;
        playerStatus.OnDefStatChanged -= UpdateDefense;
        playerStatus.OnDexStatChanged -= UpdateDex;
        playerStatus.OnIntStatChanged -= UpdateInt;

        GrowthSystem.OnSoulChanged -= UpdateSoul;
    }
    public void UpdateMaxHP(string numberText)
    {
        hpText.text = numberText;
    }
    public void UpdateAttack(string numberText)
    {
        atkText.text = numberText;
    }
    public void UpdateDefense(string numberText)
    {
        defText.text = numberText;
    }
    public void UpdateDex(string numberText)
    {
        dexText.text = numberText;
    }
    public void UpdateInt(string numberText)
    {
        intText.text = numberText;
    }
    public void UpdateSoul(string count)
    {
        soulText.text = count;
    }
    private void OnPressExitBtn()
    {
        statBar.SetActive(false);
        player.CanReceiveInput = true;
    }
    
}
