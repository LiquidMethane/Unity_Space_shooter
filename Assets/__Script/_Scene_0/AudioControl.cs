using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControl : MonoBehaviour
{
    static public AudioControl AC;
    static Dictionary<string, AudioClip> audios;

    [Header("Set In Inspector")]
    public AudioSource audioSource;
    public AudioClip blast;
    public AudioClip hit;
    public AudioClip machineGun;
    public AudioClip standardWeapon;
    public AudioClip blasterWeapon;
    public AudioClip shieldBlop;
    public AudioClip failure;
    public AudioClip levelUp;
    public AudioClip reload;
    public AudioClip BGMBoss;
    public AudioClip victory;

    

    // Start is called before the first frame update
    void Awake()
    {
        if (AC == null) //assign AC to this if AC does not exist
            AC = this;
        else
            Debug.LogError("AudioControl.Awake() - Attempted to assign second AudioControl.AC");

        audios = new Dictionary<string, AudioClip>(); //load audio names and audioclips in to dictionary
        audios.Add("blast", blast);
        audios.Add("hit", hit);
        audios.Add("machineGun", machineGun);
        audios.Add("standardWeapon", standardWeapon);
        audios.Add("blasterWeapon", blasterWeapon);
        audios.Add("shieldBlop", shieldBlop);
        audios.Add("levelUp", levelUp);
        audios.Add("failure", failure);
        audios.Add("victory", victory);
        audios.Add("reload", reload);
        audios.Add("BGMBoss", BGMBoss);
    }

    public void PlayOneShot(string audio, float lowRange = 1f, float highRange = 1f) //play audioclip once
    {
        if (lowRange == highRange)
            AC.audioSource.PlayOneShot(audios[audio], lowRange);
        else
            AC.audioSource.PlayOneShot(audios[audio], Random.Range(lowRange, highRange));
    }

    public void setAudioSourceClip(string audio) //set audioSource.clip
    {
        AC.audioSource.clip = audios[audio];
    }

    public void Play() //play audioSource.clip
    {
        AC.audioSource.Play();
    }

    public void Stop()
    {
        AC.audioSource.Stop();
    }
    
}
