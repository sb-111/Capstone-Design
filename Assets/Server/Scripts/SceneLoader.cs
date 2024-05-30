using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class SceneLoader : MonoBehaviourPun
{
    public GameObject loaderUI;
    public Slider progressSlider;
    public static SceneLoader instance = null;
    //½Ì±ÛÅæ ÀÌ¿ë
    void Awake()
    {
        if (instance == null) instance = this;
    }

    public void LoadScene()
    {
        StartCoroutine(LoadScene_Coroutine());
    }
    public IEnumerator LoadScene_Coroutine()
    {
        progressSlider.value = 0;
        loaderUI.SetActive(true);
        float progress = 0;
        while(!PhotonNetwork.IsConnected)
        {
            progress = Mathf.MoveTowards(progress, PhotonNetwork.LevelLoadingProgress, Time.deltaTime*0.5f);
            progressSlider.value = progress;
            if (progress >= 0.9f)
            {
                progressSlider.value = 1;
            }
            yield return null;
        }
    }
}
