
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class DialogueComponent : MonoBehaviour
{
    [Header("Settings")]
    public bool PlayOnStart = false;
    public bool OnlyTriggerOnce = false;
    public DialogueScriptSO CutsceneScript;

    [Header("Playables")]
    public CinematicsController CinCon;

    public void Start()
    {
        if (CinCon == null)
            CinCon = GetComponent<CinematicsController>();
        if (PlayOnStart)
        {
            Play();
        }
    }
    public void Play()
    {
        if (enabled && CutsceneScript != null)
        {
            UIController.main.cinCon = CinCon;
            UIController.main.dialogueController.PlayCutscene(CutsceneScript);
            if (OnlyTriggerOnce)
                enabled = false;
        }
    }
    public void PlayScript(DialogueScriptSO Script)
    {
        if (enabled && Script != null)
        {
            UIController.main.cinCon = CinCon;
            UIController.main.dialogueController.PlayCutscene(Script);
        }
    }

}