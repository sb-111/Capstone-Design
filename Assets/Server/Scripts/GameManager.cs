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
    public GameObject winPanel;
    GameObject playerObj;
    private int gameMode=06895;
    [Header("게임 설정")]
    [SerializeField] public int setTime = 100;
    [SerializeField] public int setDefenceTime = 40;
    public int mode = 0; //0 : 파밍 1: 디펜스 
    public bool isPlaying = false;
    public GameObject spawner;
    GameObject portalspawner;
    GameObject portalspawner2;
    int num = 0;
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
        Cursor.lockState = CursorLockMode.Locked;
        // gameOver.enabled = false;
        PV = GetComponent<PhotonView>();
        overPanel.SetActive(false);
        winPanel.SetActive(false);
        playerPrefab = CharacterSelect.character;
        cameraObj = GameObject.Find("TPS Camera");
       
        //playerSpawnPoint = playerSpawnPoints[(PhotonNetwork.LocalPlayer.ActorNumber - 1) % 3];
        playerSpawnPoint = playerSpawnPoints[0];
        mapObj = GameObject.Find("CanvasMiniMap");
        if (playerPrefab == null)
        {
            Debug.LogError("프레팹 없음");
        }
        else
        {

            spawn();
        }
        GameStart();
    }
    public void TeleportPlayer(Transform receiver, GameObject player) 
    {
        PV.RPC("TeleportPlayer", RpcTarget.All, receiver.position, player);
    }

    [PunRPC]
    public void RPCTeleportPlayer(Vector3 receiver, GameObject player)
    {
        Debug.Log("포탈 테스트 들어가지나3" + PV.IsMine + player);
        if ( player != null)
        {
            Debug.Log("포탈 테스트 들어가지나2" + player.transform.position + receiver);
            player.transform.position = receiver;
        }
    }

    public void SpawnerOn()
    {
        PV.RPC("RPCSpawnerOn", RpcTarget.All);
    }
  
    public void SpawnerOff()
    {
        PV.RPC("RPCSpawnerOff", RpcTarget.All);
    }

    [PunRPC]
    public void RPCSpawnerOn()
    {
        spawner.gameObject.SetActive(true);
    }
    [PunRPC]
    public void RPCSpawnerOff()
    {
        spawner.gameObject.SetActive(false);
    }
    public CameraShake SetEffect()
    {
  
        return cameraObj.GetComponent<CameraShake>();
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
 
    public void ObjDelete(GameObject obj)
    {
        PhotonNetwork.Destroy(obj.gameObject);
    }

    public void GameStart()
    {
        Debug.Log(PV.IsMine  +"포톤 테스트"+PhotonNetwork.IsMasterClient);
        if (PV.IsMine&&PhotonNetwork.IsMasterClient&&num==0)
        {
            SpawnManager.Instance.TimerSpawn();
            portalspawner2 = SpawnManager.Instance.PortalSpawnerSpawn();
        }
        PV.RPC("StartSetting", RpcTarget.All);
        num++;
    }
    public void PlayerReset()
    {
        if (playerObj.GetComponent<PhotonView>().IsMine) 
            PhotonNetwork.Destroy(playerObj);
        spawn();
    }
    public void DefenceStart()
    {
        PV.RPC("StartDefence", RpcTarget.All);
       // SpawnManager.Instance.TimerDestroy();
        //setTime = setDefenceTime;
       // SpawnManager.Instance.TimerSpawn();
        //SpawnManager.Instance.portalSpawn();
    }

    public void PlayerDead()
    {
        overPanel.SetActive(true);
        Destroy(portalspawner);
        //gameOver.enabled = true;

        //gameOver.text = "YOU DIED";
        //Invoke("spawn", respawnTime);
    }
    void spawn()
    {
        playerObj = PhotonNetwork.Instantiate(this.playerPrefab.name, playerSpawnPoints[PhotonNetwork.LocalPlayer.ActorNumber%2].transform.position, Quaternion.identity);
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
    public bool IsMaster()
    {
     
        if (photonView.IsMine)
             return true;
        else
            return false;
    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.P))
        {

            PlayerReset();
        }
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            Cursor.lockState = CursorLockMode.None;
            Debug.Log("눌림");
        }
        else if (mode == 1)
            return;
        /*
        else if (Input.GetKeyDown(KeyCode.O))
        {
            if (!isPlaying)
                GameStart();

            else
                Debug.Log("세명 아님/게임 시작중");


        }
        */
    }
    [PunRPC]
    public void RPCDEF()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("PortalSpawner");
        if (objs!= null)
        {
            foreach (GameObject obj in objs)
            {
                if(obj != null)
                    Destroy(obj);
            }
        }
    }

    public void Defencefail()
    {
        PV.RPC("RPCDEF", RpcTarget.All);
        //PhotonNetwork.Destroy(portalspawner);
        portalspawner = null;
        PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Winner", "none" } });
        portalspawner =SpawnManager.Instance.PortalSpawnerSpawn();
        mode = 0;
        SpawnerOn();
        setTime = 45;
        SpawnManager.Instance.TimerSpawn();

    }

    [PunRPC]
    public void GameOver(int mode)
    {
        mode = 0;
        Debug.Log("승패 확인"+ PhotonNetwork.CurrentRoom.CustomProperties["Winner"] + "챙겼나"+PV.Owner.NickName);
        isGameover = true;
        isPlaying = false;
        overPanel.SetActive(true);
        gameOver.enabled = true;
        gameOver.text = "종료";
        string a = PhotonNetwork.CurrentRoom.CustomProperties["Winner"].ToString();
        string b = PhotonNetwork.NickName;
        if (a==b)
        {
          
            winPanel.SetActive(true);

        }
        else
        {
            overPanel.SetActive(true);
        }
    }
    [PunRPC]
    public void StartSetting()
    {
        mode = 0;
        isPlaying = true;

    }
    [PunRPC]
    public void StartDefence()
    {
        mode = 1;
        
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

