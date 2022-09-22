using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Variable Change", menuName = "Dialogue/Variable Change")]
public class ScriptableDialogueVariableChange : ScriptableDialogue
{
    public VariableManager.Condition[] Changes;
    public override IEnumerator Asd(DialogueUIController DC)
    {
        GameController.main.variables.Apply(Changes);
        yield return null;
    }
}
