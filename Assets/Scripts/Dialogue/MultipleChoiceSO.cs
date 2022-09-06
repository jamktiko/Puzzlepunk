using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MultipleChoiceSO", menuName = "Dialogue/MultipleChoiceSO")]
public class MultipleChoiceSO : ChoiceSO
{
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
