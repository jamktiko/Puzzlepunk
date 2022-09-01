
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{

    [Header("Settings")]
    public bool PlayOnStart = false;
    public bool OnlyTriggerOnce = false;
    public DialogueScriptSO CutsceneScript;

    public void Start()
    {
        if (PlayOnStart)
        {
            Play();
        }
    }
    public void Play()
    {
        if (enabled)
            StartCoroutine(CinemaCoroutine());
    }

    public IEnumerator CinemaCoroutine()
    {
        /*yield return new WaitUntil(() =>
        {
            return PlayerClass.main != null;
        });*/
        OnEnterCinematic();
        DialogueUIController.main.PlayCutscene(CutsceneScript);

            yield return new WaitUntil(() => {
                return !DialogueUIController.main.IsCutscenePlaying();
            });
            OnExitCinematic();
            if (OnlyTriggerOnce)
                enabled = false;
    }

    public void OnEnterCinematic()
    {
       // WindowController.main.DialogueMenuWindow.SetActive(true);
       // PlayerClass.main.SetCinematicMode(true);
    }

    public void OnExitCinematic()
    {
       // WindowController.main.DialogueMenuWindow.SetActive(false);
       // PlayerClass.main.SetCinematicMode(false);
    }
}