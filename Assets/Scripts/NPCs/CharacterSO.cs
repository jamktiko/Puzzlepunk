using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPC", menuName = "Dialogue/NPC File")]
public class CharacterSO : ScriptableObject
{
    [Header("Character File")]
    public DialogueCharacterSO CharacterFile;

    [Header("Welcome Player")]
    public DialogueLineSO WelcomeLine;
    public DialogueLineSO Question;

    public DialogueScriptSO StandardReply;
    public NPCReaction[] Reactions;
    [System.Serializable]
    public class NPCReaction
    {
        public string VariableReaction;
        public DialogueScriptSO ReactionDialogue;
    }
}
