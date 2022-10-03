using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MultipleChoiceSO;

[CreateAssetMenu(fileName = "Random Line", menuName = "Dialogue/Components/Random Line")]
public class RandomDialogueSO : DialogueLineSO
{
    [Header("Randomly Selected")]
    public string[] PossibleChoices;
    public override string GetDialogueLine()
    {
        if (PossibleChoices!=null && PossibleChoices.Length > 0)
        { 
        return PossibleChoices[Random.Range(0,PossibleChoices.Length-1)];
        }
        return base.GetDialogueLine();
    }
}
