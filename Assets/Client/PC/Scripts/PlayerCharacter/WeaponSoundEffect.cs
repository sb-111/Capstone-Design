using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSoundEffect : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip[] hitMonsterSounds;
    public AudioClip[] hitWoodSounds;
    public AudioClip[] hitStoneSounds;
    public AudioClip[] hitWeaponSounds;
    public AudioClip[] SwingSounds;
    public AudioClip[] HeavySwingSounds;
    private AudioSource audioSource;
    
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public void PlayWeaponSound(string tag)
    {
        AudioClip clipToPlay = null;

        switch (tag)
        {
            case "Monster":
                clipToPlay = hitMonsterSounds[Random.Range(0,hitMonsterSounds.Length)];
                break;
            case "Wood":
                clipToPlay = hitWoodSounds[Random.Range(0, hitWoodSounds.Length)];
                break;
            case "Stone":
                clipToPlay = hitStoneSounds[Random.Range(0, hitStoneSounds.Length)];
                break;
            case "Weapon":
                clipToPlay = hitWeaponSounds[Random.Range(0, hitWeaponSounds.Length)];
                break;
            case "Swing":
                clipToPlay = SwingSounds[Random.Range(0,SwingSounds.Length)];
                break;
            case "HeavySwing":
                clipToPlay = HeavySwingSounds[Random.Range(0, HeavySwingSounds.Length)];
                break;
        }

        if(clipToPlay != null)
        {
            audioSource.PlayOneShot(clipToPlay);
        }
    }
}
