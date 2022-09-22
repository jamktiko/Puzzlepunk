using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Play Sound", menuName = "Dialogue/Play Sound")]
public class ScriptableDialogueSound : ScriptableDialogue
{
    public AudioClip Clip;
    public override IEnumerator Asd(DialogueUIController DC)
    {
        DC.PlaySound(Clip);
        yield return null;
    }
}
