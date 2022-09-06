using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class InteractableBase : MonoBehaviour
{
    new Collider2D collider;
    public bool RequiresMoveInRange = false;
    public float InteractRange = 1f;
    public bool SingleUse = false;
    public virtual void OnInteract()
    {
        if (SingleUse)
            EnableDisable(false);
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
        if (collider!=null)
        {
            collider.enabled = value;
        }
    }
}
