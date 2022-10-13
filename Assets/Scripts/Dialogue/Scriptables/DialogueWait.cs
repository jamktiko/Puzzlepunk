using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wait", menuName = "Dialogue/Components/Wait")]
public class DialogueWait : ScriptableDialogue
{
    [Header("Waiting")]
    public float WaitTime;
    public bool Skippable = true;
    public override IEnumerator Run(DialogueUIController DC)
    {
        if (WaitTime > 0)
        {
            if (Skippable)
                yield return DC.PostLineWait(WaitTime);
            else
                yield return new WaitForSeconds(WaitTime);
        }
    }
}
