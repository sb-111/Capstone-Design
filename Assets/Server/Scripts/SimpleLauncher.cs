using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

using Photon.Realtime;

public class SimpleLauncher : MonoBehaviourPunCallbacks
{

    public PhotonView playerPrefab;
    bool isConnecting;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Connect()
    {

        //SceneLoader.instance.LoadScene(1);
        PhotonNetwork.ConnectUsingSettings();
        isConnecting = true;
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        if (isConnecting)
        {
            PhotonNetwork.JoinRandomRoom();
        }
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
        PhotonNetwork.LoadLevel("ServerTestScene");//纠 捞抚
        Debug.Log("规 甸绢皑");
        // PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0, 1, 0), Quaternion.identity);

    }

}
