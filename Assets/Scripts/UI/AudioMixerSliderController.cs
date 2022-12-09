using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMixerSliderController : MonoBehaviour
{
    public static Dictionary<string, float> defaultValues = new Dictionary<string, float>();

    public string PlayerPrefSlot;
    public AudioMixer AudioGroup;
     float ParameterDefaultValue = 0;
     float ParameterMinValue = -20;

    private void Start()
    {
        if (defaultValues.ContainsKey(PlayerPrefSlot))
        {
            ParameterDefaultValue = defaultValues[PlayerPrefSlot];
        }
        else if (AudioGroup.GetFloat(PlayerPrefSlot, out float value))
        {
            ParameterDefaultValue = value;
            defaultValues.Add(PlayerPrefSlot, value);
        }
        else
        {
            ParameterDefaultValue = 0;
        }
        InitVolume();
    }
    void InitVolume()
    {
        if (ParameterDefaultValue == 0)
            return;
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
        {
            float rVolume = nVolume == 0 ? -40 : (ParameterMinValue + (ParameterDefaultValue - ParameterMinValue) * nVolume);
            AudioGroup.SetFloat(PlayerPrefSlot, rVolume);
        }
        PlayerPrefs.SetFloat(AudioGroup.name + PlayerPrefSlot, nVolume);
    }
}
