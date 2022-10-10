using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Change Variable", menuName = "Dialogue/Components/Change Variable")]
public class ScriptableDialogueVariableChange : ScriptableDialogue
{
    public VariableManager.Set[] Changes;
    public override void OnSkipped(DialogueUIController DC)
    {
        GameController.main.variables.Apply(Changes);
    }
    public override IEnumerator Run(DialogueUIController DC)
    {
        OnSkipped(DC);
        yield return null;
    }
}
