using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Line", menuName = "Dialogue/Components/Line")]
public class DialogueLineSO : DialogueWait
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
    public override IEnumerator Run(DialogueUIController DC)
    {
        DC.LoadDialogueCharacter(this);
        string DialogueQuestion = GetDialogueLine();
        yield return DC.TypeDialog(DialogueQuestion);
        if (WaitTime > 0)
        {
            yield return base.Run(DC);
        }
        else
        {
            if (Skippable)
                yield return DC.PostLineWait(DialogueQuestion);
            else
                yield return new WaitForSeconds(WaitTime);
        }
    }
}
