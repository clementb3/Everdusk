using UnityEngine;
using UnityEngine.UI;

public class MenuAudioManager : MonoBehaviour
{
    public GameObject panelMenuPrincipal; // Référence au panel principal
    public GameObject panelAudio;        // Référence au panel audio

    // Sliders pour ajuster le volume
    public Slider sliderVolumeGlobal;
    public Slider sliderVolumeSFX;
    public Slider sliderVolumeBGM;

    // Toggle pour activer/désactiver l'audio
    public Toggle toggleAudio;

    // AudioMixer 
    public UnityEngine.Audio.AudioMixer audioMixer;

    void Start()
    {
        sliderVolumeGlobal.onValueChanged.AddListener(SetVolumeGlobal);
        sliderVolumeSFX.onValueChanged.AddListener(SetVolumeSFX);
        sliderVolumeBGM.onValueChanged.AddListener(SetVolumeBGM);
        toggleAudio.onValueChanged.AddListener(ToggleAudio);
    }

    public void OpenAudioSettings()
    {
        panelAudio.SetActive(true);
        panelMenuPrincipal.SetActive(false);
    }

    public void CloseAudioSettings()
    {
        panelAudio.SetActive(false);
        panelMenuPrincipal.SetActive(true);
    }

    public void SetVolumeGlobal(float volume)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
    }

    public void SetVolumeSFX(float volume)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
    }

    public void SetVolumeBGM(float volume)
    {
        audioMixer.SetFloat("BGMVolume", Mathf.Log10(volume) * 20);
    }

    public void ToggleAudio(bool isOn)
    {
        AudioListener.volume = isOn ? 1 : 0;
    }
}
