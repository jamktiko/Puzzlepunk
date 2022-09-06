using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChoiceSO : ScriptableObject
{
    [Header("Character Data")]
    public DialogueCharacterSO Character;

    [Header("Emote")]
    public DialogueScriptSO.CharacterEyePosition Eyes;
    public DialogueScriptSO.CharacterMouthPosition Mouth;

    [Header("Dialoge")]
    public string DialogueQuestion;
}
