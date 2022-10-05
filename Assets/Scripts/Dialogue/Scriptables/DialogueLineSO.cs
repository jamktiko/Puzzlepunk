using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Line", menuName = "Dialogue/Components/Line")]
public class DialogueLineSO : ScriptableDialogue
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
    public float WaitTime;

    public virtual string GetDialogueLine()
    {
        return DialogueQuestion;
    }
    public override IEnumerator Run(DialogueUIController DC)
    {
        DC.LoadDialogueCharacter(this);
        string DialogueQuestion = GetDialogueLine();
        yield return DC.TypeDialog(DialogueQuestion);
        if (WaitTime>0)
            yield return DC.PostLineWait(WaitTime);
        else 
        yield return DC.PostLineWait(DialogueQuestion);
    }
}
