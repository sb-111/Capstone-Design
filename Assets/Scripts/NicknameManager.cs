using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NicknameManager : MonoBehaviour
{
    public InputField nicknameInputField; // InputField 컴포넌트를 할당받을 변수
    private string nickname; // 사용자의 닉네임을 저장할 변수

    public void SaveNickname()
    {
        nickname = nicknameInputField.text; // InputField의 값을 nickname 변수에 저장
        Debug.Log("닉네임이 저장되었습니다: " + nickname); // 콘솔에 저장된 닉네임 출력
    }
}
