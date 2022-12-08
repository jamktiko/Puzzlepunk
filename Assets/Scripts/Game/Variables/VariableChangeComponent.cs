
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class VariableChangeComponent : MonoBehaviour
{
    public VariableManager.Set[] Changes;
    public void ApplyChanges()
    {
        GameController.main.variables.Apply(Changes);
    }
}