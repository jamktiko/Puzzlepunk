using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogLineSO", menuName = "Dialogue/DialogLineSO")]
public class DialogueLineSO : ScriptableObject
{
    public enum CharacterEmotion
    {
        none = -1,
        normal = 0,
        smile = 1,
        laugh = 2,
        ask = 3,
        think = 4,
        worry = 5,
    }
    [Header("Character")]
    public DialogueCharacterSO Character;
    public CharacterEmotion Emote;

    [Header("Dialoge")]
    public string DialogueQuestion;

    public virtual string GetDialogueLine()
    {
        return DialogueQuestion;
    }
}
