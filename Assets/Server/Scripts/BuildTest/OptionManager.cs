using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionManager : MonoBehaviour
{
    [SerializeField] private Button exitButton;
    [SerializeField] private Toggle fullTog;
    [SerializeField] private TextMeshProUGUI ScreenText;
    [SerializeField] private Slider vSlider;
    [SerializeField] private Slider hSlider;
    [SerializeField] private GameObject optionBar;
    [SerializeField] private GameObject statBar;
    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        if (Screen.fullScreen)
            fullTog.isOn = true;
        else
            fullTog.isOn = false;
        ScreenText.text = "1920X1080";
    }
    void Awake()
    {
        exitButton.onClick.AddListener(OnPressExitBtn);
        fullTog.onValueChanged.AddListener(delegate { FullToggle(); });
        vSlider.onValueChanged.AddListener(delegate { OnVSlide(vSlider.value); });
        hSlider.onValueChanged.AddListener (delegate { OnHSlide(hSlider.value); });
        
        player = GetComponentInParent<Player>();    
    }
    public void SetScreen1920()
    {
        Screen.SetResolution(1920, 1080, true);
        ScreenText.text = "1920X1080";
    }
    public void SetScreen1366()
    {
        Screen.SetResolution(1366, 768, true);
        ScreenText.text = "1366X768";
    }
    public void SetScreen1440()
    {
        Screen.SetResolution(1440, 900, true);
        ScreenText.text = "1440X900";
    }
    public void SetScreen2560()
    {
        Screen.SetResolution(2560, 1440, true);
        ScreenText.text = "2560X1440";
    }
    public void FullToggle()
    {

        Screen.fullScreen = !Screen.fullScreen;
       
    }

    private void OnVSlide(float value)
    {
        Debug.Log($"수직 감도:{value}");
        player.SetVSensivity(value);
    }
    private void OnHSlide(float value)
    {
        player.SetHSensivity(value);
    }
    private void OnPressExitBtn()
    {
        optionBar.SetActive(false);
        player.CanReceiveInput = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !statBar.activeSelf)
        {
            optionBar.SetActive(!optionBar.activeSelf);
            player.CanReceiveInput = !optionBar.activeSelf;
        }
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    if (option.activeSelf)
        //    {
        //        Application.Quit();
        //    }
        //    else
        //    {
        //        option.SetActive(true);
        //    }
        //}
    }
    private void OnDestroy()
    {
        exitButton.onClick.RemoveAllListeners();
        fullTog.onValueChanged.RemoveAllListeners();
        vSlider.onValueChanged.RemoveAllListeners();
        hSlider.onValueChanged.RemoveAllListeners();
    }
}
