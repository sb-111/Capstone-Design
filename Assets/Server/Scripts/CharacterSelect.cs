using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

using Photon.Realtime;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviourPunCallbacks
{
    public Toggle cha1tog;
    public Toggle cha2tog;
    public static GameObject character;
    public GameObject chara1;
    public GameObject chara2;

    // Start is called before the first frame update
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


    // Update is called once per frame
    void Update()
    {
        
    }
    public void GameStart() 
    {
        PhotonNetwork.JoinRandomRoom();
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("规积己");

        //规 积己
        PhotonNetwork.CreateRoom(null, new RoomOptions());
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined a room.");

        PhotonNetwork.LoadLevel("MainScene");//纠 捞抚
    
        //StartCoroutine(LoadLevelWithProgress("MainScene"));

        Debug.Log("规 甸绢皑");
        // PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0, 1, 0), Quaternion.identity);

    }


}
