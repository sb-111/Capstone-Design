using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBGM : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip mainClip; // Lobbyscene과 Mainmenuscene에서 재생할 클립
    public AudioClip defenceClip; // Mainscene에서 재생할 클립

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
        if (GameManager.Instance.mode == 1&& audioSource.clip != defenceClip)
        {
            audioSource.clip = defenceClip;
            audioSource.Play();
        }
    }
}
