using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioMixerSliderController : MonoBehaviour
{
    public AudioMixer AudioGroup;

    public void ChangeVolume (float nVolume)
    {
        if (AudioGroup != null)
            AudioGroup.SetFloat("MasterVolume", nVolume);
    }
}
