using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Countdown : MonoBehaviour
{
    [SerializeField] int setTime = 100;
    [SerializeField] Text countdownText;
    [SerializeField] GameObject Player;
    int playerCount = 0;
    int mode = 0;
    private int time;
    private PhotonView PV;

    private bool portalSpawned = false;

    void Start()
    {
        PV = GetComponent<PhotonView>();
        /*int initialMinutes = Mathf.FloorToInt(setTime / 60);
        int initialSeconds = Mathf.FloorToInt(setTime - initialMinutes * 60);
        countdownText.text = string.Format("{0:00}:{1:00}", initialMinutes, initialSeconds);
    */
    }

    void Update()
    {
        playerCount = PhotonNetwork.PlayerList.Length;
        if (mode == 0)
        {
            if (PhotonNetwork.IsMasterClient && !portalSpawned && playerCount == 2)
            {
                mode = 1;
                PV.RPC("StartTimerCoroutine", RpcTarget.All);
                //StartCoroutine("TimerCoroutine");
                Debug.Log("timertest");
            }
        }
    }

    void OnTriggerEnter(Collider coll)
    {
        Debug.Log("Ãæµ¹");
        if (coll.gameObject.tag == "Portal" && mode == 1)
        {
            setTime = 20;
            mode = 2;
            countdownText.color = Color.red;
            StartCoroutine("TimerCoroutine");
            Debug.Log("Open Portal");
        }
    }

    IEnumerator TimerCoroutine()
    {
        while (setTime > 0)
        {
            if (mode == 2)
            {
                mode = 3;
                yield break;
            }
            setTime -= 1;
            PV.RPC("ShowTimer", RpcTarget.All, setTime);
            yield return new WaitForSeconds(1);
        }
        portalSpawned = true;
        Debug.Log("timer finish");
        mode = 1;
        yield break;
    }

    [PunRPC]
    private void ShowTimer(int setTime)
    {
        Debug.Log("timertest12");
        int minutes = Mathf.FloorToInt(setTime / 60);
        int seconds = Mathf.FloorToInt(setTime - minutes * 60);
        countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    [PunRPC]
    void StartTimerCoroutine()
    {
        StartCoroutine("TimerCoroutine");
    }
}

