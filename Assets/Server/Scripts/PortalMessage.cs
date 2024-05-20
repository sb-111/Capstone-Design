using UnityEngine;
using UnityEngine.UI;

public class PortalMessage : MonoBehaviour
{
    public Text messageText;  // UI Text 요소를 연결합니다.

    void Start()
    {
        messageText.enabled = false;  // 시작할 때 텍스트를 비활성화합니다.
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  // 플레이어가 포탈에 접근하면
        {
            messageText.text = "다음 층으로 이동하시겠습니까?";
            messageText.enabled = true;  // 텍스트를 활성화합니다.
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))  // 플레이어가 포탈에서 멀어지면
        {
            messageText.enabled = false;  // 텍스트를 비활성화합니다.
        }
    }
}
