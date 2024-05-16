using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISoul : MonoBehaviour
{
    [SerializeField] private Text soulText;
    // Start is called before the first frame update
    void Start()
    {
        soulText.text = "0";
    }
    public void UpdateUI(string quantity)
    {
        soulText.text = quantity;
    }

}
