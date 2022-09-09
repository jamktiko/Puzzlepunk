using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ClueChoiceSO", menuName = "Dialogue/ClueChoiceSO")]
public class ClueChoiceSO : DialogueLineSO
{
    public string[] VariablesRequired;
    public DialogueScriptSO SuccessDialogue;
}
