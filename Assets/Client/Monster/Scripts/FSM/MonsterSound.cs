using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSound : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip[] audioClips; // 오디오 클립 배열
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
   
    public void PlaySound(int index)
    {
        audioSource.PlayOneShot(audioClips[index]);
    }
    
}
