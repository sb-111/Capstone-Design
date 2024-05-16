using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    public Image fadeImage; // 검정 화면을 나타낼 Image 컴포넌트를 할당할 변수
    public float fadeDuration = 1f; // 페이드 지속 시간

    private void Start()
    {
        if (fadeImage == null)
        {
            fadeImage = GetComponent<Image>();
        }
    }

    // 화면을 검정으로 페이드 아웃
    public void FadeToBlack()
    {
        StartCoroutine(Fade(1f));
    }

    // 화면을 검정에서 페이드 인
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
