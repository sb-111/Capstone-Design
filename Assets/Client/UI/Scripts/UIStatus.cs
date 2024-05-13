using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStatus : MonoBehaviour
{
    public void SetUI()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
