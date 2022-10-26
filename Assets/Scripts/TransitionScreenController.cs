using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TransitionScreenController : MonoBehaviour
{
    public Image FadeOutImage;
    private void Awake()
    {
        if (FadeOutImage == null)
        {
            FadeOutImage = GetComponent<Image>();
        }
    }
    private void Start()
    {
        StartCoroutine(FadeOut(0));
    }

    Coroutine TransitionCoroutine;

    public void TransitionIn(float dur)
    {
        StartCoroutine(FadeIn(dur));
    }
    public IEnumerator AwaitTransitionIn(float dur)
    {
        yield return StartCoroutine(FadeIn(dur));
    }
    public IEnumerator FadeIn(float dur)
    {
        Color FOIcolor = FadeOutImage.color;

        FOIcolor.a = 0;
        while (FOIcolor.a < 1)
        {
            FOIcolor.a += Time.deltaTime / dur;
            FadeOutImage.color = FOIcolor;
            yield return new WaitForEndOfFrame();
        }
        FOIcolor.a = 1;
        FadeOutImage.color = FOIcolor;
    }
    public void TransitionOut(float dur)
    {
         StartCoroutine(FadeOut(dur));
    }
    public IEnumerator AwaitTransitionOut(float dur)
    {
        yield return StartCoroutine(FadeOut(dur));
    }
    public IEnumerator FadeOut(float dur)
    {
        Color FOIcolor = FadeOutImage.color;

        FOIcolor.a = 1;
        while (FOIcolor.a > 0)
        {
            FOIcolor.a -= Time.deltaTime / dur;
            FadeOutImage.color = FOIcolor;
            yield return new WaitForEndOfFrame();
        }
        FOIcolor.a = 0;
        FadeOutImage.color = FOIcolor;
    }
}
