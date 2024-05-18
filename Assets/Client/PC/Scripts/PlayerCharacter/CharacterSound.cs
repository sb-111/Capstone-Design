using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSound : MonoBehaviour
{
    public AudioClip[] walkSounds_forest;
    public AudioClip[] walkSounds_desert;
    public AudioClip[] walkSounds_cave;

    public AudioClip[] rollSounds;
    public AudioClip[] attackSoudns;
    public AudioClip[] eKeySounds;
    public AudioClip[] deadSoudns;
    public AudioSource audioSource;

    float volume;
    float pitch;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public void PlayCharacterSound(string tag)
    {
        AudioClip clipToPlay = null;
        
        switch(tag)
        {
            case "Roll":
                
                clipToPlay = rollSounds[0];
                break;
            case "Attack":                  //각 공격에 따른 목소리 출력으로 바꿔야 함
                clipToPlay = attackSoudns[Random.Range(0, attackSoudns.Length)];
                break;
            case "EKey":
                clipToPlay = eKeySounds[Random.Range(0, eKeySounds.Length)];
                break;
            case "Dead":
                clipToPlay = deadSoudns[Random.Range(0, deadSoudns.Length)];    
                break;
        }

        if(clipToPlay != null)
        {
            audioSource.volume = 1.0f;
            audioSource.pitch = 1.0f;
            audioSource.Stop();
            audioSource.PlayOneShot(clipToPlay);
        }
    }


    public void PlayWalkSound()
    {
        if (!audioSource.isPlaying)
        {
            if (transform.position.x < -500)
            {
                audioSource.clip = walkSounds_desert[0];
                volume = 0.2f;
                pitch = 1.2f;
            }
            else if (transform.position.x > 650) 
            {
                audioSource.clip = walkSounds_cave[0];
                volume = 0.2f;
                pitch = 1.0f;
            }
            else {
                audioSource.clip = walkSounds_forest[0];
                volume = 0.2f;
                pitch = 0.65f;
            }
            audioSource.volume = volume;
            audioSource.pitch = pitch;
            audioSource.Play();           
        }
    }


    public void IsRunning(bool rDown)
    {
        if (rDown) {
            audioSource.volume = volume*2.5f;
            audioSource.pitch = pitch*1.25f;
        }
        else
        {
            audioSource.volume = volume;
            audioSource.pitch = pitch;
        }

    }


    public void StopFootsteps()
    {
        if(audioSource.isPlaying) {
            audioSource.Stop();
            audioSource.volume = 1.0f;
            audioSource.pitch = 1.0f;
        }
    }
}
