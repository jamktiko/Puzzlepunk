using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMixerSliderController : MonoBehaviour
{
    public string PlayerPrefSlot;
    public AudioMixer AudioGroup;
     float ParameterDefaultValue = 0;
     float ParameterMinValue = -40;

    private void Start()
    {
        if (AudioGroup.GetFloat(PlayerPrefSlot, out float value))
        {
            ParameterDefaultValue = value;
        }
        else
        {
            ParameterDefaultValue = 0;
        }
        InitVolume();
    }
    void InitVolume()
    {
        float volume = 1;
        if (PlayerPrefs.HasKey(AudioGroup.name + PlayerPrefSlot))
        {
            volume = PlayerPrefs.GetFloat(AudioGroup.name + PlayerPrefSlot);
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
            AudioGroup.SetFloat(PlayerPrefSlot, ParameterMinValue + (ParameterDefaultValue - ParameterMinValue) * nVolume);   
        PlayerPrefs.SetFloat(AudioGroup.name + PlayerPrefSlot, nVolume);
    }
    private void OnDestroy()
    {
        AudioGroup.SetFloat(PlayerPrefSlot, ParameterDefaultValue);
    }
}
