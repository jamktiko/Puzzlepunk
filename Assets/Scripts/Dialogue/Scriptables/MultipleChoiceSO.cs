using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Multiple Choice", menuName = "Dialogue/Components/Multiple Choice")]
public class MultipleChoiceSO : DialogueLineSO
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
    public override IEnumerator Run(DialogueUIController DC)
    {
        DC.ShowMultipleChoice(this);
        yield return new WaitWhile(()=> { return true; });
    }
}
