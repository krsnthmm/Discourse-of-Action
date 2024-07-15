using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("[VOLUME MANAGEMENT]")]
    public AudioMixer mixer;
    public Slider masterSlider;
    public Slider BGMSlider;
    public Slider SFXSlider;

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
    
    void Start()
    {
        if (instance == null)
        {
            instance = this;

            masterSlider.value = PlayerPrefsManager.Load("MasterVolume");
            BGMSlider.value = PlayerPrefsManager.Load("BGMVolume");
            SFXSlider.value = PlayerPrefsManager.Load("SFXVolume");

            PlayClip(BGMSource, menuBGM);
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

    public void SetMasterVolume()
    {
        mixer.SetFloat("MasterVolume", masterSlider.value);
        PlayerPrefsManager.Save("MasterVolume", masterSlider.value);
    }

    public void SetBGMVolume()
    {
        mixer.SetFloat("BGMVolume", BGMSlider.value);
        PlayerPrefsManager.Save("BGMVolume", BGMSlider.value);
    }

    public void SetSFXVolume()
    {
        mixer.SetFloat("SFXVolume", SFXSlider.value);
        PlayerPrefsManager.Save("SFXVolume", SFXSlider.value);
    }
}
