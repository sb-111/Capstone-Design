using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    public Image fadeImage; // ���� ȭ���� ��Ÿ�� Image ������Ʈ�� �Ҵ��� ����
    public float fadeDuration = 1f; // ���̵� ���� �ð�

    private void Start()
    {
        if (fadeImage == null)
        {
            fadeImage = GetComponent<Image>();
        }
    }

    // ȭ���� �������� ���̵� �ƿ�
    public void FadeToBlack()
    {
        StartCoroutine(Fade(1f));
    }

    // ȭ���� �������� ���̵� ��
    public void FadeFromBlack()
    {
        StartCoroutine(Fade(0f));
    }

    private IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = fadeImage.color.a;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            Color color = fadeImage.color;
            color.a = newAlpha;
            fadeImage.color = color;
            yield return null;
        }

        Color finalColor = fadeImage.color;
        finalColor.a = targetAlpha;
        fadeImage.color = finalColor;
    }
}
