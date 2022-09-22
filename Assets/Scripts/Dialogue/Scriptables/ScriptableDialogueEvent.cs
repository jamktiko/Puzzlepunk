using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Event", menuName = "Dialogue/Event")]

public class ScriptableDialogueEvent : ScriptableDialogue
{
    public UnityEvent Event;
    public override IEnumerator Run(DialogueUIController DC)
    {
        Event.Invoke();
        yield return null;
    }
}
