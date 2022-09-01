using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueScriptSO", menuName = "Dialogue/DialogueCharacter")]
public class DialogueCharacterSO : ScriptableObject
{

    public string CharacterName;

    public Sprite Base;
    [Header("Eyes")]
    public Sprite[] Eyes;
    [Header("Mouth")]
    public Sprite[] Mouths;
}
