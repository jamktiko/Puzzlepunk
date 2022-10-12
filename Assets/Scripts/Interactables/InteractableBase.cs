using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class InteractableBase : MonoBehaviour
{
    public UnityEvent uEvent;
    new Collider2D collider;
    public bool SingleUse = false;
    bool usable = true;
    public virtual void OnInteract()
    {
        if (!CanBeInteractedWith())
            return;
            if (SingleUse)
            EnableDisable(false);
        uEvent.Invoke();
    }
    public virtual void Awake()
    {
        if (collider == null)
        {
            collider = GetComponent<Collider2D>();
        }
    }
    public virtual void EnableDisable(bool value)
    {
        usable = value;
    }
    public bool CanBeInteractedWith()
    {
        return usable;
    }
}

