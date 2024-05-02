using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SimpleLauncher : MonoBehaviourPunCallbacks
{

    public PhotonView playerPrefab;
    bool isConnecting;

    public Slider loadingProgressBar;
    public GameObject loadingUI;

    // Start is called before the first frame update
    void Start()
    {
        //Connect();
    }

    public void OnLoginButtonClicked()
    {
        loadingUI.SetActive(true);
        Connect();
    }
    public void Connect()
    {

        //SceneLoader.instance.LoadScene(1);

        isConnecting = true;
        PhotonNetwork.ConnectUsingSettings();

        // 로딩 UI 활성화
        //loadingUI.SetActive(true);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        if (isConnecting)
        {
            SceneManager.LoadScene("MainMenuScene");
        }
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("방생성");

        //방 생성
        PhotonNetwork.CreateRoom(null, new RoomOptions());
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined a room.");

        LoadScene("MainScene");


        Debug.Log("방 들어감");
        // PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0, 1, 0), Quaternion.identity);

    }

    void LoadScene(string scene)
    {
        StartCoroutine(LoadScene_Coroutine(scene));
    }
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


    IEnumerator LoadLevelWithProgress(string sceneName)
    {
        // 로딩 UI 활성화
        //loadingUI.SetActive(true);

        // Scene 비동기 로딩 시작
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName); // SceneManager를 사용
        asyncLoad.allowSceneActivation = false;
        loadingProgressBar.value = 0;
        float targetProgress = 0;
        float fillSpeed = 0.5f;
        //float lerpSpeed = 10f;
        // 로딩이 완료될 때까지 대기
        while (!asyncLoad.isDone)
        {
            // 프로그레스 바 업데이트
            //float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            if(asyncLoad.progress < 0.9f)
            {
            targetProgress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            loadingProgressBar.value = Mathf.MoveTowards(loadingProgressBar.value, targetProgress, fillSpeed * Time.deltaTime);
            }
            else
            {
                loadingProgressBar.value = Mathf.MoveTowards(loadingProgressBar.value, 1.0f, fillSpeed * Time.deltaTime);
                if(loadingProgressBar.value >= 1.0f)
                {
                    asyncLoad.allowSceneActivation = true;
                }
            }

            yield return null;
        }

        // 로딩 완료 후 로딩 UI 비활성화
        loadingUI.SetActive(false);
    }

}
