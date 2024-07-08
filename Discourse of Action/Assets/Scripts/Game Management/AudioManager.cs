using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("[AUDIO SOURCES]")]
    public AudioSource BGMSource;
    public AudioSource SFXSource;
    public AudioSource voiceSource;

    [Header("[BACKGROUND MUSIC]")]
    public AudioClip menuBGM;
    public AudioClip introBGM;
    public AudioClip gameBGM;
    public AudioClip combatBGM;
    public AudioClip bossBGM;
    public AudioClip memoryRecallBGM;

    [Header("[JINGLES]")]
    public AudioClip combatWonJingle;
    public AudioClip memoryRecallCompleteJingle;

    [Header("[SOUND EFFECTS]")]
    public AudioClip buttonSFX;

    [Header("[VOICE]")]
    public AudioClip femIntro;
    public AudioClip mascIntro;
    public AudioClip femCombatEnd;
    public AudioClip mascCombatEnd;
    
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            PlayClip(BGMSource, menuBGM); // this should only work for the main menu
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void PlayClip(AudioSource audioSrc, AudioClip clip)
    {
        audioSrc.clip = clip;
        audioSrc.Play();
    }

    public void StopPlayback(AudioSource audioSrc)
    {
        audioSrc.Stop();
    }
}
