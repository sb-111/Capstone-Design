using UnityEngine;
using UnityEngine.UI;

public class RandomRawImage : MonoBehaviour
{
    public RawImage displayRawImage; // UI���� �̹����� ǥ���� RawImage ������Ʈ
    public Texture[] textures; // ����� �ؽ�ó���� ������ �迭

    void Start()
    {
        ChangeTexture();        
    }
    // ��ư�� ������ �� ȣ��Ǵ� �Լ�
    public void ChangeTexture()
    {
        int index = Random.Range(0, textures.Length); // ������ �ε��� ����
        displayRawImage.texture = textures[index]; // ���õ� �ε����� �ؽ�ó�� ����
    }
}
