
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


     void Start()
    {
        if (PlayOnStart)
        {
            Play();
        }
    }
    public virtual void Play()
    {
        UIController.main.dialogueController.PlayCutscene(CutsceneScript);
    }
    public void PlayScript(DialogueScriptSO Script)
    {
        if (enabled && Script != null)
        {
            UIController.main.dialogueController.PlayCutscene(Script);
            if (OnlyTriggerOnce)
                enabled = false;
        }
    }

}