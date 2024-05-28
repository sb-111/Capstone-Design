using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using TMPro;

public class CharacterSelect : MonoBehaviourPunCallbacks
{
    public Toggle cha1tog;
    public Toggle cha2tog;
    public static GameObject character;
    public GameObject chara1;
    public GameObject chara2;
    public GameObject charaPanel;
    public GameObject roomPanel;
    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI charnametxt;
    public GameObject chartxt1;
    public GameObject chartxt2;
    private int charnum = 0;
    private string playerName;
    private int playerCount = 0;
    public GameObject chaimg1;
    public GameObject chaimg2;
    // 추가된 UI 요소
    public Slider loadingProgressBar;
    public GameObject loadingUI;

    void awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.AddCallbackTarget(this);
        //miniMapController = FindObjectOfType<CanvasMiniMap>().GetComponent<MiniMapController>; // MiniMapController의 인스턴스 가져오기
    }
    void Start()
    {
        roomPanel.SetActive(false);
        charaPanel.SetActive(true);
        loadingUI.SetActive(false);
        chaimg1.SetActive(true);
        chaimg2.SetActive(false);
        character = chara1;

        playerName = PhotonNetwork.LocalPlayer.NickName + "\n";
    }
    void Update()
    {
        switch (charnum)
        {
            case 0:
                charnametxt.text = "Warrior";
                chaimg1.SetActive(true);
                chaimg2.SetActive(false);
                chartxt1.SetActive(true);
                chartxt2.SetActive(false);
                character = chara1;
                break;
            case 1:
                charnametxt.text = "Assassin";
                chaimg1.SetActive(false);
                chaimg2.SetActive(true);
                chartxt1.SetActive(false);
                chartxt2.SetActive(true);
                character = chara2;
                break;
            default:
                break;
        }
    }
    public void Lbtn()
    {
        if (charnum < 1)
        {
            charnum++;
        }
        else
            charnum = 0;
    }
    public void Rbtn()
    {
        if (charnum > 0)
        {
            charnum--;
        }
        else
            charnum = 1;

    }




    public void GameStart()
    {

        PhotonNetwork.JoinRandomRoom();
        roomPanel.SetActive(true);
        charaPanel.SetActive(false);
        playerNameText.text = playerName;
        loadingUI.SetActive(true);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("방생성");
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 3;
        roomOptions.CustomRoomProperties = new Hashtable() { { "Winner", "없음" } };

        PhotonNetwork.CreateRoom(null, roomOptions);
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {

        playerName += newPlayer.ActorNumber + "\n";
        Debug.Log("들어옴");
        playerNameText.text = playerName;

        if (PhotonNetwork.CurrentRoom.PlayerCount == 3)
        {
            loadingUI.SetActive(true);
            roomPanel.SetActive(false);
            charaPanel.SetActive(false);
            //StartCoroutine(LoadScene_Coroutine("MainScene"));
            //PhotonNetwork.LoadLevel("MainScene");

        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined a room.");

        //StartCoroutine(LoadScene_Coroutine("MainScene"));
        // 코루틴을 사용하여 씬 로딩 시작
        StartCoroutine(LoadLevel_Coroutine("MainScene"));
        //PhotonNetwork.LoadLevel("MainScene");
    }

    // SimpleLauncher에서 가져온 LoadScene_Coroutine
    IEnumerator LoadLevel_Coroutine(string scene)
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
    IEnumerator LoadScene_Coroutine(string scene)
    {
        loadingProgressBar.value = 0;
        loadingUI.SetActive(true);
        float progress = 0;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            progress = Mathf.MoveTowards(progress, asyncLoad.progress, Time.deltaTime * 0.5f);
            loadingProgressBar.value = progress;
            if (progress >= 0.9f)
            {
                loadingProgressBar.value = 1;
                if (asyncLoad.progress >= 0.9f)
                {
                    asyncLoad.allowSceneActivation = true;
                }
            }
            yield return null;
        }

        // 씬 로드가 완료된 후에 loadingUI를 비활성화합니다.
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
    void OnDestroy()
    {

        PhotonNetwork.RemoveCallbackTarget(this);
    }
}