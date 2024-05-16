using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameManager : MonoBehaviourPunCallbacks
{
    [Header("플레이어 설정")]
    //[SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private GameObject playerSpawnPoint;
    [SerializeField]
    private GameObject[] playerSpawnPoints;
    [SerializeField]
    private float respawnTime = 10f;
    public bool isGameover = false;
    private PhotonView PV;
    public bool portalOwner = false;
    private static GameManager instance = null;
    private GameObject cameraObj;
    private GameObject mapObj;
    public TextMeshProUGUI gameOver;
    public GameObject overPanel;
    private int gameMode=06895;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

    }
  
    void Start()
    {
        // gameOver.enabled = false;
        PV = GetComponent<PhotonView>();
        overPanel.SetActive(false);
        playerPrefab = CharacterSelect.character;
        cameraObj = GameObject.Find("TPS Camera");

        playerSpawnPoint = playerSpawnPoints[(PhotonNetwork.LocalPlayer.ActorNumber - 1) % 3];
        mapObj = GameObject.Find("CanvasMiniMap");
        if (playerPrefab == null)
        {
            Debug.LogError("프레팹 없음");
        }
        else
        {

            spawn();

        }
    }
    public static GameManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    public void GetPortal(string winnerPlayerID)
    {
        // CustomProperties를 사용하여 우승자 정보를 전달
    
        PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Winner", winnerPlayerID } });
        Debug.Log(PhotonNetwork.CurrentRoom.CustomProperties["Winner"] + "승패 확인" + winnerPlayerID + "챙겼나" + PV.Owner.NickName);
    }
  

    public void PlayerDead()
    {
        overPanel.SetActive(true);
        gameOver.enabled = true;
        gameOver.text = "YOU DIED";
        Invoke("spawn", respawnTime);
    }
    void spawn()
    {
        GameObject playerObj = PhotonNetwork.Instantiate(this.playerPrefab.name, playerSpawnPoint.transform.position, Quaternion.identity);
        playerObj.name = "player: "+PV.Owner.NickName;

        if (cameraObj != null)
        {
            CameraFollow camaraFollow = cameraObj.GetComponent<CameraFollow>();
            if (camaraFollow != null)
            {
                camaraFollow.SetPlayer(playerObj);
            }
        }
        if (mapObj != null)
        {
            MiniMapController miniMapController = mapObj.GetComponent<MiniMapController>();
            if (miniMapController != null)
            {
                miniMapController.SetPlayer(playerObj);
            }
        }
        Debug.Log("확인");
        overPanel.SetActive(false);
        gameOver.enabled = false;
        isGameover = false;
        // 리스폰 동작 실행
        // 여기에 리스폰에 관련된 코드를 작성합니다.
    }
    public void GameFinish()
    {
        Debug.Log("게임 종료");
        PV.RPC("GameOver", RpcTarget.All,gameMode);
        Debug.Log("승패 확인" + PhotonNetwork.CurrentRoom.CustomProperties["Winner"] + "챙겼나" + PV.Owner.NickName);
    }

    [PunRPC]
    public void GameOver(int mode)
    {
        Debug.Log("승패 확인"+ PhotonNetwork.CurrentRoom.CustomProperties["Winner"] + "챙겼나"+PV.Owner.NickName);
        isGameover = true;
        overPanel.SetActive(true);
        gameOver.enabled = true;
        gameOver.text = "종료";
        string a = PhotonNetwork.CurrentRoom.CustomProperties["Winner"].ToString();
        string b = PV.Owner.NickName;
        if (a==b)
        {
            if (PV.IsMine)
            gameOver.text = "WIN";
            else
                gameOver.text = "LOSE";
        }
        else
        {
            gameOver.text = "LOSE";
        }
    }
    void LoadArena()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            return;
        }
        Debug.LogFormat("PhotonNetwork : Loading Level : ServerTestScene", PhotonNetwork.CurrentRoom.PlayerCount);
        PhotonNetwork.LoadLevel("MainScene");
    }

    public GameObject GetPlayerObject()
    {
        
        return playerPrefab; 
    }

}

