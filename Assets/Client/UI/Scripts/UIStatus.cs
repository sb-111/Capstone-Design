using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStatus : MonoBehaviour
{
    //[SerializeField] private PlayerStatus status;
    [SerializeField] private GrowthSystem growthSystem;
    //private GrowthSystem growthSystem;
    [SerializeField] private GameObject statBar;
    [SerializeField] private Button hpEnhanceButton;
    [SerializeField] private Button atkEnhanceButton;
    [SerializeField] private Button defEnhanceButton;
    [SerializeField] private Button dexEnhanceButton;
    [SerializeField] private Button intEnhanceButton;
    private void Start()
    {
        //growthSystem = FindObjectOfType<GrowthSystem>();
        
        // 버튼의 모든 리스너 삭제
        hpEnhanceButton.onClick.RemoveAllListeners();
        atkEnhanceButton.onClick.RemoveAllListeners();
        defEnhanceButton.onClick.RemoveAllListeners();
        dexEnhanceButton.onClick.RemoveAllListeners();
        intEnhanceButton.onClick.RemoveAllListeners();

        // 인스턴스 ID 출력
        Debug.Log("UIStatus에서 참조한 growthSystem ID: " + growthSystem.GetInstanceID());

        //if (growthSystem!=null)
        //{
        //    if (growthSystem.GetDicCnt()>0)
        //    {
        //        Debug.Log("개수"+growthSystem.GetDicCnt());
        //    }
        //}

        // 리스너 등록
        hpEnhanceButton.onClick.AddListener(growthSystem.OnPressEnhanceHealthBtn);
        atkEnhanceButton.onClick.AddListener(growthSystem.OnPressEnhanceAttackBtn);
        defEnhanceButton.onClick.AddListener(growthSystem.OnPressEnhanceDefenseBtn);
        dexEnhanceButton.onClick.AddListener(growthSystem.OnPressEnhanceDexBtn);
        intEnhanceButton.onClick.AddListener(growthSystem.OnPressEnhanceIntBtn);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("호출됨");
            statBar.SetActive(!statBar.activeSelf);
        }
    }
    
}
