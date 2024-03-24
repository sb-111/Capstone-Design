using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Countdown : MonoBehaviour
{
    [SerializeField] int setTime = 100;
    [SerializeField] Text countdownText;
    //[SerializeField] GameObject Player;
    //[SerializeField] SpawnPortal spawnPortal;
    int playerCount = 0;
    public static int mode = 0;
    private int time;
    private int PortalMode = 0;
    private PhotonView PV;

    private bool portalSpawned = false;

    void Start()
    {
        PV = GetComponent<PhotonView>();
        int initialMinutes = Mathf.FloorToInt(setTime / 60); // 시작할 때의 분
        int initialSeconds = Mathf.FloorToInt(setTime - initialMinutes * 60); // 시작할 때의 초
        countdownText.text = string.Format("{0:00}:{1:00}", initialMinutes, initialSeconds); // 시작할 때의 시간을 텍스트로 설정
    }
  
    void Update()
    {
        playerCount = PhotonNetwork.PlayerList.Length;
        /*
        if (Player != null && playerCount == 1 && !portalSpawned)
        {
            StartCoroutine(DelayedSpawn(spawnPortal.spawnDelay)); 
            portalSpawned = true;
        }

        if (playerCount == 2)
        {
            if (setTime > 0)
            {
                setTime -= Time.deltaTime;
            }
            else if (setTime <= 0)
            {
                Time.timeScale = 0.0f;
            }
            int minutes = Mathf.FloorToInt(setTime / 60F);
            int seconds = Mathf.FloorToInt(setTime - minutes * 60);
            countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        */
      

        if (mode == 0)
        {
            if (PhotonNetwork.IsMasterClient && !portalSpawned && playerCount == 1)
            {
                
                mode = 1;
               //PV.RPC("ModeChange", RpcTarget.All, mode);
                StartCoroutine("TimerCoroutine");
                Debug.Log("timertest");
                
            }
        }
        if (mode == 2) {
            if (PortalMode==0)
            {
                PortalMode= 1;
                setTime = 20;
                PV.RPC("ModeChange", RpcTarget.All, mode);
                countdownText.color = Color.red;
                StartCoroutine("TimerCoroutine");
                Debug.Log("Open Portal");

            }
            
        }
    }

 


    IEnumerator TimerCoroutine()
    {
        while (setTime > 0)
        {
            if (PortalMode==1)
            {
                PortalMode = 2;
                
                yield break;
            }
            
            setTime -= 1;
            PV.RPC("ShowTimer", RpcTarget.All, setTime);
            yield return new WaitForSeconds(1);
        }
        portalSpawned = true;
        Debug.Log("timer finish");
        //종료시 동작
        //mode = 1;
        //PV.RPC("ModeChange", RpcTarget.All, mode);
        yield break;

       
    

    }

    [PunRPC]
    private void ModeChange(int modenum)
    {
        mode = modenum;
        Debug.Log("모드 변경 확인"+mode);
        if (mode == 2)
        {
            Debug.Log("모드 2 변경 확인");
            countdownText.color = Color.red;
            PortalMode = 1;
        }
        


    }


    [PunRPC]
    private void ShowTimer(int setTime)
    {
        // 모든 클라이언트에서 호출되어 타이머를 동기화
       // Debug.Log("timertest12");
        int minutes = Mathf.FloorToInt(setTime / 60);
        int seconds = Mathf.FloorToInt(setTime - minutes * 60);
        countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        //countdownText.text = timerValue.ToString();
    }


    IEnumerator DelayedSpawn(float delay)
    {
        yield return new WaitForSeconds(delay); // 지정 시간 대기
        //spawnPortal.SpawnObject(); 
    }

}