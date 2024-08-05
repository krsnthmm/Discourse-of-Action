using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("[VOLUME MANAGEMENT]")]
    public AudioMixer mixer;

    [Header("[AUDIO SOURCES]")]
    public AudioSource BGMSource;
    public AudioSource SFXSource;

    [Header("[BACKGROUND MUSIC]")]
    public AudioClip menuBGM;
    public AudioClip introBGM;
    public AudioClip gameBGM;
    public AudioClip combatBGM;
    public AudioClip bossBGM;
    public AudioClip memoryRecallBGM;

    [Header("[JINGLES]")]
    public AudioClip combatWonJingle;
    public AudioClip combatLostJingle;
    public AudioClip memoryRecallCompleteJingle;

    [Header("[SOUND EFFECTS]")]
    public AudioClip buttonSFX;
    public AudioClip dialogueSFX;
    public AudioClip opponentTriggerSFX;
    public AudioClip superEffectiveSFX;
    public AudioClip normalEffectiveSFX;
    public AudioClip notEffectiveSFX;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;

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

    public void StopClip(AudioSource audioSrc)
    {
        audioSrc.Stop();
    }

    public void StopPlayback(AudioSource audioSrc)
    {
        audioSrc.Stop();
    }
}
