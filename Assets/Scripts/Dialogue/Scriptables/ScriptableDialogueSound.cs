using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Play Sound", menuName = "Dialogue/Components/Play Sound")]
public class ScriptableDialogueSound : ScriptableDialogue
{
    public AudioClip Clip;
    public override IEnumerator Run(DialogueUIController DC)
    {
        DC.PlaySound(Clip);
        yield return null;
    }
}
