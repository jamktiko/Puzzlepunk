using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableBase : MonoBehaviour
{
    public bool RequiresMoveInRange = false;
    public float InteractRange = 1f;
    public virtual void OnInteract()
    {

    }
}
