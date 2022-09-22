
using UnityEngine;
using UnityEngine.Events;

public class VariableReactionChange : MonoBehaviour
{
    public VariableManager.Condition[] Conditions;
    public UnityEvent Action;
    void Start()
    {
        GameController.main.variables.Reactors.Add(this);
    }
}
