using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Countdown : MonoBehaviour
{
    [SerializeField] float setTime = 100.0f;
    [SerializeField] Text countdownText;
    [SerializeField] GameObject Player;
    [SerializeField] SpawnPortal spawnPortal;
    int playerCount = 0;

    private bool portalSpawned = false;

    void Start()
    {
        int initialMinutes = Mathf.FloorToInt(setTime / 60F); // 시작할 때의 분
        int initialSeconds = Mathf.FloorToInt(setTime - initialMinutes * 60); // 시작할 때의 초
        countdownText.text = string.Format("{0:00}:{1:00}", initialMinutes, initialSeconds); // 시작할 때의 시간을 텍스트로 설정
    }

    void Update()
    {
        playerCount = PhotonNetwork.PlayerList.Length;

        if (Player != null && playerCount == 2 && !portalSpawned)
        {
            StartCoroutine(DelayedSpawn(spawnPortal.spawnDelay)); // 코루틴을 사용하여 일정 시간 뒤에 포탈을 생성하도록 합니다.
            portalSpawned = true; // 포탈이 생성되었음을 표시합니다.
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
    }

    // 일정 시간 뒤에 포탈을 생성하는 코루틴입니다.
    IEnumerator DelayedSpawn(float delay)
    {
        yield return new WaitForSeconds(delay); // 지정된 시간만큼 대기합니다.
        spawnPortal.SpawnObject(); // 대기 시간이 끝나면 포탈을 생성합니다.
    }

}