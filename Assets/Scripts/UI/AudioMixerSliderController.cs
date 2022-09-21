using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMixerSliderController : MonoBehaviour
{
    public string PlayerPrefSlot;
    public AudioMixer AudioGroup;

    private void Start()
    {
        InitVolume();
    }
    void InitVolume()
    {
        float volume = 1;
        if (PlayerPrefs.HasKey(PlayerPrefSlot))
        {
            volume = PlayerPrefs.GetFloat(PlayerPrefSlot);
        }
        if (TryGetComponent(out Slider SliderComponent))
        {
            SliderComponent.SetValueWithoutNotify( volume);
        }
        ChangeVolume(volume);
    }

    public void ChangeVolume (float nVolume)
    {
        if (AudioGroup != null)
            AudioGroup.SetFloat("MasterVolume", nVolume);   //TODO audio mixer groups
        PlayerPrefs.SetFloat(PlayerPrefSlot, nVolume);
    }
}
