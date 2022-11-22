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
        EndTransition();
        StartCoroutine(FadeIn(dur));
    }
    public IEnumerator AwaitTransitionIn(float dur)
    {
        EndTransition();
        TransitionCoroutine = StartCoroutine(FadeIn(dur));
               yield return TransitionCoroutine;
        
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
        EndTransition();
        StartCoroutine(FadeOut(dur));
    }
    public IEnumerator AwaitTransitionOut(float dur)
    {
        EndTransition();
        TransitionCoroutine = StartCoroutine(FadeOut(dur));
            yield return TransitionCoroutine;
       
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
    void EndTransition()
    {
        if (TransitionCoroutine != null)
            StopCoroutine(TransitionCoroutine);
        TransitionCoroutine = null;
    }
}
