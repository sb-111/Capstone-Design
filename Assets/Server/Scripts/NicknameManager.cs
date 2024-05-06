using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class NicknameManager : MonoBehaviour
{
    public InputField nicknameInputField; // InputField 컴포넌트를 할당받을 변수
    const string nickname ="name"; // 사용자의 닉네임을 저장할 변수
    public void SaveNickname(string value)
    {
        
        
        if (string.IsNullOrEmpty(value))
        {
            Debug.LogError("이름 없음");
            return;
        }

        PhotonNetwork.NickName = value;
        //nickname = value;
        PlayerPrefs.SetString(nickname, value);
        Debug.Log("닉네임이 저장되었습니다: " + PhotonNetwork.NickName); // 콘솔에 저장된 닉네임 출력
    }
    void Start()
    {

        string defaultName = string.Empty;
        nicknameInputField = this.GetComponent<InputField>();
        if (nicknameInputField != null)
        {
            if (PlayerPrefs.HasKey(nickname))
            {
                defaultName = PlayerPrefs.GetString(nickname);
                nicknameInputField.text = defaultName;
            }
        }

        PhotonNetwork.NickName = defaultName;
    }
}
