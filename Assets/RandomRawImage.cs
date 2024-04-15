using UnityEngine;
using UnityEngine.UI; // UI 관련 기능을 사용하기 위해 필요합니다.

public class RandomRawImage : MonoBehaviour
{
    public RawImage displayRawImage; // UI에서 이미지를 표시할 RawImage 컴포넌트.
    public Texture[] textures; // 사용할 텍스처들을 저장할 배열.

    void Start()
    {
        ChangeTexture();        
    }
    // 버튼을 눌렀을 때 호출되는 함수.
    public void ChangeTexture()
    {
        int index = Random.Range(0, textures.Length); // 랜덤한 인덱스 선택.
        displayRawImage.texture = textures[index]; // 선택된 인덱스의 텍스처로 변경.
    }
}
