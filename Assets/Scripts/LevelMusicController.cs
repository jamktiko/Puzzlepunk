using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMusicController : MonoBehaviour
{
    public AudioSource audioSource;

    private void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();  
    }

    Coroutine ChangeMusicCoroutine;
    public void SmoothTransition(AudioClip nMusic)
    {
        if (ChangeMusicCoroutine != null)
        {
            StopCoroutine(ChangeMusicCoroutine);
            audioSource.volume = startVolume;
        }
        ChangeMusicCoroutine = StartCoroutine(ChangeMusicTransition(1,nMusic));
    }
    float startVolume = 1f;
    public IEnumerator ChangeMusicTransition(float duration, AudioClip nMusic)
    {
        startVolume = audioSource.volume;
        for (float fadeOut = duration; fadeOut > 0; fadeOut -= Time.deltaTime)
        {
            audioSource.volume = startVolume * fadeOut / duration;
            yield return new WaitForEndOfFrame();
        }
        audioSource.Stop();
        if (nMusic != null)
        {
            audioSource.clip = nMusic;
            audioSource.Play();
            for (float fadeIn = 0; fadeIn < duration; fadeIn += Time.deltaTime)
            {
                audioSource.volume = startVolume * fadeIn / duration;
                yield return new WaitForEndOfFrame();
            }
        }
        audioSource.volume = startVolume;
    }
}
