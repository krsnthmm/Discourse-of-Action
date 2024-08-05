using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    public Slider masterSlider;
    public Slider BGMSlider;
    public Slider SFXSlider;

    // Start is called before the first frame update
    void Start()
    {
        masterSlider.value = PlayerPrefsManager.Load("MasterVolume");
        BGMSlider.value = PlayerPrefsManager.Load("BGMVolume");
        SFXSlider.value = PlayerPrefsManager.Load("SFXVolume");
    }
    public void SetMasterVolume()
    {
        AudioManager.instance.mixer.SetFloat("MasterVolume", masterSlider.value);
        PlayerPrefsManager.Save("MasterVolume", masterSlider.value);
    }

    public void SetBGMVolume()
    {
        AudioManager.instance.mixer.SetFloat("BGMVolume", BGMSlider.value);
        PlayerPrefsManager.Save("BGMVolume", BGMSlider.value);
    }

    public void SetSFXVolume()
    {
        AudioManager.instance.mixer.SetFloat("SFXVolume", SFXSlider.value);
        PlayerPrefsManager.Save("SFXVolume", SFXSlider.value);
    }

    public void OnSliderUp()
    {
        AudioManager.instance.PlayClip(AudioManager.instance.SFXSource, AudioManager.instance.buttonSFX);
    }
}
