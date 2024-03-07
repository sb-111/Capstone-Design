using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SimpleLauncher : MonoBehaviourPunCallbacks
{

    public PhotonView playerPrefab;
    public PhotonView timer;

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined a room.");
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0, 1, 0), Quaternion.identity);
       //PhotonNetwork.Instantiate(timer.name, new Vector3(0, 1, 0), Quaternion.identity);
    }

}