using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviourPunCallbacks
{
    [Header("플레이어 설정")]
    //[SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private GameObject playerSpawnPoint;
    [SerializeField]
    private float respawnTime = 10f;
    public bool isGameover { get; private set; }

    public bool portalOwner = false;
    private static GameManager instance = null;
  
    public TextMeshProUGUI gameOver;
    public GameObject overPanel;
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
        overPanel.SetActive(false);
        playerPrefab = CharacterSelect.character;
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
    public void GetPortal()
    {
        portalOwner = true;
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
        GameObject cameraObj = GameObject.Find("TPS Camera");
        if (cameraObj != null)
        {
            CameraFollow camaraFollow = cameraObj.GetComponent<CameraFollow>();
            if (camaraFollow != null)
            {
                camaraFollow.SetPlayer(playerObj);
            }
        }
        Debug.Log("확인");
        overPanel.SetActive(false);
        isGameover = false;
        // 리스폰 동작 실행
        // 여기에 리스폰에 관련된 코드를 작성합니다.
    }
    public void GameFinish()
    {
        Debug.Log("게임 종료");
        isGameover = true;
        gameOver.enabled = true;
        if (portalOwner)
        {
           gameOver.text = "WIN";
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
 


}

