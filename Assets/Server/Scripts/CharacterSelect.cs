using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSelect : MonoBehaviourPunCallbacks
{
    public Toggle cha1tog;
    public Toggle cha2tog;
    public static GameObject character;
    public GameObject chara1;
    public GameObject chara2;

    // 추가된 UI 요소
    public Slider loadingProgressBar;
    public GameObject loadingUI;

    void Start()
    {
        character = chara1;
        cha1tog.onValueChanged.RemoveAllListeners();
        cha1tog.onValueChanged.AddListener(OnChara1);

        cha2tog.onValueChanged.RemoveAllListeners();
        cha2tog.onValueChanged.AddListener(OnChara2);
    }

    void OnChara1(bool _bool)
    {
        if (true)
        {
            character = chara1;
        }
    }

    void OnChara2(bool _bool)
    {
        if (true)
        {
            character = chara2;
        }
    }

    public void GameStart()
    {
        loadingUI.SetActive(true);
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("방생성");
        PhotonNetwork.CreateRoom(null, new RoomOptions());
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined a room.");
        // 코루틴을 사용하여 씬 로딩 시작
        StartCoroutine(LoadScene_Coroutine("MainScene"));
    }

    // SimpleLauncher에서 가져온 LoadScene_Coroutine
    IEnumerator LoadScene_Coroutine(string scene)
    {
        loadingProgressBar.value = 0;
        loadingUI.SetActive(true);
        float progress = 0;
        PhotonNetwork.LoadLevel(scene);
        while (!PhotonNetwork.IsConnected)
        {
            progress = Mathf.MoveTowards(progress, PhotonNetwork.LevelLoadingProgress, Time.deltaTime * 0.5f);
            loadingProgressBar.value = progress;
            if (progress >= 0.9f)
            {
                loadingProgressBar.value = 1;
            }
            yield return null;
        }
        loadingUI.SetActive(false);
    }

    // SimpleLauncher에서 가져온 LoadLevelWithProgress
    IEnumerator LoadLevelWithProgress(string sceneName)
    {
        loadingUI.SetActive(true);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;
        loadingProgressBar.value = 0;
        float targetProgress = 0;
        float fillSpeed = 0.5f;
        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress < 0.9f)
            {
                targetProgress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
                loadingProgressBar.value = Mathf.MoveTowards(loadingProgressBar.value, targetProgress, fillSpeed * Time.deltaTime);
            }
            else
            {
                loadingProgressBar.value = Mathf.MoveTowards(loadingProgressBar.value, 1.0f, fillSpeed * Time.deltaTime);
                if (loadingProgressBar.value >= 1.0f)
                {
                    asyncLoad.allowSceneActivation = true;
                }
            }
            yield return null;
        }
        loadingUI.SetActive(false);
    }
}
