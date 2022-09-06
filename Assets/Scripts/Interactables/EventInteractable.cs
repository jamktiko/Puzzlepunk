using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventInteractable : InteractableBase
{
    public UnityEvent uEvent;
    public override void OnInteract()
    {
        uEvent.Invoke();
        base.OnInteract();
    }
}
