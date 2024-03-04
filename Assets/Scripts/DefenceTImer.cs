using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
//시험용 타이머 RPC를 이용 초가 변할 때만 동기화 더 정확한 시간 계측이 필요하면 방법 변경 필요.
public class DefenceTImer : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI timerUI;
    private int time;
    private PhotonView PV;

    void Start()
    {
     
    }

    public override void OnConnectedToMaster()
    {
        PV = GetComponent<PhotonView>();
        //Debug.Log("timertest");
        // MasterClient에서만 타이머 시작
       if (PhotonNetwork.IsMasterClient)
       {
            time = 10;
            StartCoroutine("TimerCoroutine");
            Debug.Log("timertest");
        }
  
    }



    IEnumerator TimerCoroutine()
    {
        while (time > 0)
        {
        
            time -= 1;
            PV.RPC("ShowTimer", RpcTarget.All, time);
            yield return new WaitForSeconds(1);
        }
     
            Debug.Log("timer finish");
            yield break;

            //종료시 동작

        
    }

    //동기화 받아서 표시
    [PunRPC]
    private void ShowTimer(int timerValue)
    {
        // 모든 클라이언트에서 호출되어 타이머를 동기화
        Debug.Log("timertest12");
        timerUI.text = timerValue.ToString();
    }


    /*
  // Update is called once per frame
  void Update()
  {
      if (PhotonNetwork.IsMasterClient)
      {
          // Master Client일 경우 타이머 값을 갱신하고 동기화
          if(time > 0) {
              time -= Time.deltaTime;
              playerPrefab.RPC("SyncTimer", RpcTarget.AllBuffered, time);
          }
          else
          {
              //타이머 종료후 작동할 코드 ㄴ여기에 넣음 
          }

      }


  }
    
       void timer()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
            timerUI.text = time.ToString("#.##");
        }
        else
        {
            timerUI.text = "finish";   
        }

    }
     */

}
