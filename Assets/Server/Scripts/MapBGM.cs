using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBGM : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip mainClip; // Lobbyscene과 Mainmenuscene에서 재생할 클립
    public AudioClip defenceClip; // Mainscene에서 재생할 클립
    bool isChange=false;
    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = mainClip;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.mode == 1&& !isChange)
        {
            StartCoroutine("MusicChange");
            audioSource.volume = 1f;
            isChange = true;
        }
        if (GameManager.Instance.mode == 0 && isChange)
        {
            StartCoroutine("MusicChange2");
            audioSource.volume = 1f;
            isChange = false;
        }
    }
    IEnumerator MusicChange2()
    {
        float progressTime = 0f;

        while (progressTime <= 3f)
        {
            progressTime += Time.deltaTime;
            audioSource.volume = (float)(1.0 - progressTime / 3f);
            yield return null;
        }
        progressTime = 0f;
        audioSource.clip = mainClip;
        audioSource.Play();
        while (progressTime <= 3f)
        {
            progressTime += Time.deltaTime;
            audioSource.volume = (float)(progressTime / 3f);
            yield return null;
        }
        audioSource.volume = 1f;
        yield break;

    }

    IEnumerator MusicChange()
    {
        float progressTime = 0f;
   
        while (progressTime <= 3f)
        {
            progressTime += Time.deltaTime;
            audioSource.volume = (float)(1.0 -progressTime / 3f);
            yield return null;
        }
        progressTime = 0f;
        audioSource.clip = defenceClip;
        audioSource.Play();
        while (progressTime <= 0.5f)
        {
            progressTime += Time.deltaTime;
            audioSource.volume = (float)(progressTime / 0.5f);
            yield return null;
        }
        audioSource.volume = 1f;
        yield break;
   
    }

}
