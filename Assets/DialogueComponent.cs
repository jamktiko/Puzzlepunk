using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutscenePlayer : MonoBehaviour
{
    public DialogueScriptSO Cutscene;
    public void Play()
    {
        UIController.main.dialogueController.PlayCutscene(Cutscene);
    }
}
