using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableDialogue : ScriptableObject
{
    public virtual IEnumerator Run(DialogueUIController DC)
    {
        yield return null;
    }
}
