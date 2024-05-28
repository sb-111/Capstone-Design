using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionManager : MonoBehaviour
{
    [SerializeField] private Toggle fullTog;
    [SerializeField] private TextMeshProUGUI ScreenText;
    [SerializeField] private Slider vSlider;
    [SerializeField] private Slider hSlider;
    [SerializeField] private Player player;
    // Start is called before the first frame update
    void Start()
    {
        if (Screen.fullScreen)
            fullTog.isOn = true;
        else
            fullTog.isOn = false;
    }
    void Awake()
    {
        fullTog.onValueChanged.AddListener(delegate { FullToggle(); });
        vSlider.onValueChanged.AddListener(delegate { OnVSlide(vSlider.value); });
        hSlider.onValueChanged.AddListener (delegate { OnHSlide(hSlider.value); });
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
    public void OnVSlide(float value)
    {
        Debug.Log($"슬라이드값:{value}");
        player.SetVSensivity(value);
    }
    private void OnHSlide(float value)
    {
        player.SetHSensivity(value);
    }
    // Update is called once per frame
    void Update()
    {
        ScreenText.text = "1920X1080";
    }
}
