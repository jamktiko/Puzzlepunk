
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


    public void Start()
    {
        if (PlayOnStart)
        {
            Play();
        }
    }
    public void Play()
    {
        if (enabled && CutsceneScript != null)
        {
            UIController.main.dialogueController.PlayCutscene(CutsceneScript);
            if (OnlyTriggerOnce)
                enabled = false;
        }
    }
    public void PlayScript(DialogueScriptSO Script)
    {
        if (enabled && Script != null)
        {
            UIController.main.dialogueController.PlayCutscene(Script);
        }
    }

}