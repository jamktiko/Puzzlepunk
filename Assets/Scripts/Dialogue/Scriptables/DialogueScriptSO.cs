using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Script", menuName = "Dialogue/Components/Script")]
public class DialogueScriptSO : ScriptableObject
{
    public ScriptableDialogue[] Dialogue;
}
