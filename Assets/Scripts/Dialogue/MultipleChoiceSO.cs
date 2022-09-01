using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueScriptSO", menuName = "Dialogue/MultipleChoiceSO")]
public class MultipleChoiceSO : ScriptableObject
{
    [Header("Character Data")]
    public DialogueCharacterSO Character;

    [Header("Emote")]
    public DialogueScriptSO.CharacterEyePosition Eyes;
    public DialogueScriptSO.CharacterMouthPosition Mouth;

    [Header("Choices")]
    public Choice[] Choices;

    [System.Serializable]
    public class Choice
    {
        public string name;
        public string text;
        public DialogueScriptSO dialogue;
    }
}
