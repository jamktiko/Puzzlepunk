using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChoiceSO : ScriptableObject
{
    [Header("Character")]
    public DialogueCharacterSO Character;
    public DialogueScriptSO.CharacterEmotion Emote;

    [Header("Dialoge")]
    public string DialogueQuestion;
}
