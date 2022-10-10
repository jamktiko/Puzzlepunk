using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Event", menuName = "Dialogue/Components/Event")]

public class ScriptableDialogueEvent : ScriptableDialogue
{
    public UnityEvent Event;
    public override void OnSkipped(DialogueUIController DC)
    {
        Event.Invoke();
    }
    public override IEnumerator Run(DialogueUIController DC)
    {
        OnSkipped(DC);
        yield return null;
    }
}
