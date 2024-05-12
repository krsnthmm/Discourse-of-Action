using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("[AUDIO SOURCES]")]
    [SerializeField] private AudioSource _BGMSource;
    [SerializeField] private AudioSource _SFXSource;
    [SerializeField] private AudioSource _voiceSource;

    [Header("[BACKGROUND MUSIC]")]
    [SerializeField] private AudioClip _menuBGM;
    [SerializeField] private AudioClip _gameBGM;
    [SerializeField] private AudioClip _combatBGM;
    [SerializeField] private AudioClip _bossBGM;

    [Header("[SOUND EFFECTS]")]
    [SerializeField] private AudioClip _buttonSFX;

    [Header("[VOICE]")]
    [SerializeField] private AudioClip _femIntro;
    [SerializeField] private AudioClip _mascIntro;
    [SerializeField] private AudioClip _femCombatEnd;
    [SerializeField] private AudioClip _mascCombatEnd;
    
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        PlayClip(_BGMSource, _menuBGM);
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
