using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Countdown : MonoBehaviour
{
    [SerializeField] int setTime = 100;
    [SerializeField] int setPortalTime = 40;
    [SerializeField] Text countdownText;
    //[SerializeField] GameObject Player;
    //[SerializeField] SpawnPortal spawnPortal;
    int playerCount = 0;
    public static int mode = 0;
    //모드 0 : 초기 상태
    //모드 1 : 포탈 생성 상태 
    private int time;
    private int PortalMode = 0;
    private PhotonView PV;
    private int timerStop = 0;
    private bool portalSpawned = false;
    private bool gameStarted = false;

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
  
        if (PhotonNetwork.IsMasterClient)
        {
           if(!gameStarted)
            {
                StartTimer(setTime);
                gameStarted = true;
                Debug.Log("timertest");
            }
            if (mode ==1) {

                ResetTimer(setPortalTime);
                portalSpawned = true;
                mode = 2;
                Debug.Log("포탈 생성");
            } 

        }


   
    }

    public void StartTimer(int time)
    {
        setTime = time;
        StartCoroutine("TimerCoroutine");
        Debug.Log("timertest 시작");
    }
    public void StopTimer()
    {

    }
    public void ResetTimer(int time)
    {
        timerStop = 1;
        setTime = time;
        StartCoroutine("TimerCoroutine");
        Debug.Log("타이머 리셋");
    }


    IEnumerator TimerCoroutine()
    {

        while(setTime > 0)
        {
            if (timerStop == 1)
            {
                Debug.Log("stop 받음");
                timerStop = 0;
                yield break;
            }
    
            setTime -= 1;
            PV.RPC("ShowTimer", RpcTarget.All, setTime);
            yield return new WaitForSeconds(1);
        }
            Debug.Log("타이머 종료");
        GameManager.Instance.GameFinish();

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
       Debug.Log("timertest RPC");
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