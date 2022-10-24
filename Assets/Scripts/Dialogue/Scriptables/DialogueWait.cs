using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wait", menuName = "Dialogue/Components/Wait")]
public class DialogueWait : ScriptableDialogue
{
    [Header("Waiting")]
    public float WaitTime;
    public override IEnumerator Run(DialogueUIController DC)
    {
        if (WaitTime > 0)
        {
                yield return new WaitForSeconds(WaitTime);
        }
    }
}
