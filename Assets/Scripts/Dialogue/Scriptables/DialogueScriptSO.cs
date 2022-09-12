using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueScriptSO", menuName = "Dialogue/DialogueScriptSO")]
public class DialogueScriptSO : ScriptableObject
{
    public DialogueLineSO[] Dialogue;
    public bool TriggerRoomTransition;
}
