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
        evil,
        annoyed, 
        uhoh,
        bellamylean,
        shut,
        menacing,
        lookingdowntendril,
        lookingdownhand,
        friendly,

    }
    [Header("Character")]
    public DialogueCharacterSO Character;
    public CharacterEmotion Emote;

    [Header("Dialoge")]
    public Sprite DialogueImage;
    public string DialogueQuestion;

    public virtual string GetDialogueLine()
    {
        return DialogueQuestion;
    }
    public IEnumerator LoadDialogue(DialogueUIController DC)
    {
        if (DialogueImage != null)
        {
            DC.dialogImage.sprite = DialogueImage;
            DC.dialogImage.color = Color.white;
            DC.dialogImage.enabled = true;
        }
        else
        {
            DC.dialogImage.enabled = false;
        }
        DC.LoadDialogueCharacter(this);
        string DialogueQuestion = GetDialogueLine();
        yield return DC.TypeDialog(DialogueQuestion, Character == null);
    }
    public override IEnumerator Run(DialogueUIController DC)
    {
        yield return LoadDialogue(DC);
        yield return DC.PostLineWait();
        OnSkipped(DC);
    }
    public override void OnSkipped(DialogueUIController DC)
    {
        base.OnSkipped(DC);
        //DC.dialogImage.enabled = false;
    }
}
