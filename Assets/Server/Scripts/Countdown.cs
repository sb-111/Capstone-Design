using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Countdown : MonoBehaviour
{
    [SerializeField] int setTime = 100;
    [SerializeField] int setPortalTime = 40;
    Text countdownText;
    //[SerializeField] GameObject Player;
    //[SerializeField] SpawnPortal spawnPortal;
    int playerCount = 0;
    public static int mode = 0;
    //��� 0 : �ʱ� ����
    //��� 1 : ��Ż ���� ���� 
    private int time;
    private int PortalMode = 0;
    private PhotonView PV;
    private int timerStop = 0;
    private bool portalSpawned = false;
    private bool gameStarted = false;

    void OnEnable()
    {
        PV = GetComponent<PhotonView>();
        countdownText = GameObject.Find("Countdown").GetComponent<Text>();
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

                StartPortal();
            } 

        }
   
    }

    public void StartPortal()
    {
        ResetTimer(setPortalTime);
        portalSpawned = true;
       
        SpawnManager.Instance.portalSpawn();
        mode = 2;
        Debug.Log("포탈 시작");
    }

    public void StartTimer(int time)
    {
        setTime = time;
        StartCoroutine("TimerCoroutine");
        Debug.Log("timertest ����");
    }
    public void StopTimer()
    {

    }
    public void ResetTimer(int time)
    {
        if (mode == 1)
            timerStop = 1;
        setTime = time;
        StartCoroutine("TimerCoroutine");
        Debug.Log("타이머 리셋");
    }


    IEnumerator TimerCoroutine()
    {
        if (timerStop == 1)
        {
            Debug.Log("타이머 재 시작");
            timerStop = 0;
            yield break;
        }
        while (setTime > 0)
        {
            
    
            setTime -= 1;
            PV.RPC("ShowTimer", RpcTarget.All, setTime);
            yield return new WaitForSeconds(1);
        }
            Debug.Log("타이머 종료");
        if (mode == 2) {
            GameManager.Instance.GameFinish();
            Debug.Log("종료 확인");
        
        }
        else
        {
            StartPortal();
            Debug.Log("종료 확인");
        }

        yield break;

    }

    [PunRPC]
    private void ModeChange(int modenum)
    {
        mode = modenum;
        Debug.Log("��� ���� Ȯ��"+mode);
        if (mode == 2)
        {
            Debug.Log("��� 2 ���� Ȯ��");
            countdownText.color = Color.red;
            PortalMode = 1;
        }
        


    }
   


    [PunRPC]
    private void ShowTimer(int setTime)
    {
        // ��� Ŭ���̾�Ʈ���� ȣ��Ǿ� Ÿ�̸Ӹ� ����ȭ
       Debug.Log("timertest RPC");
        int minutes = Mathf.FloorToInt(setTime / 60);
        int seconds = Mathf.FloorToInt(setTime - minutes * 60);
        countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        //countdownText.text = timerValue.ToString();
        Debug.Log("승패 확인 xkdlaj" + PhotonNetwork.CurrentRoom.CustomProperties["Winner"]);
    }



}