using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableDialogue : ScriptableObject
{
    public virtual void OnSkipped(DialogueUIController DC)
    {

    }
    public virtual IEnumerator Run(DialogueUIController DC)
    {
        yield return null;
    }
}
