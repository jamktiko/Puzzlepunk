using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableCutscene : InteractableBase
{
    public DialogueScriptSO Cutscene;
    public override void OnInteract()
    {
        UIController.main.dialogueController.PlayCutscene(Cutscene);
        base.OnInteract();
    }
}
