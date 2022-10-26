
using UnityEngine;
using UnityEngine.Events;
using static VariableManager;

public class VariableReactionChange : MonoBehaviour
{
    public VariableManager.Condition[] Conditions;
    public UnityEvent Action;
    void Start()
    {
        LevelController.main.Reactors.Add(this);
    }
}
