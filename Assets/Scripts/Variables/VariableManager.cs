using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableManager
{
    public Dictionary<string, Variable> EncodedVariables = new Dictionary<string, Variable>();
    public List<VariableReactionChange> Reactors = new List<VariableReactionChange>();
    public Variable GetVariable(string vName)
    {
        if (EncodedVariables.ContainsKey(vName))
        {
            return EncodedVariables[vName];
        }
        return SetVariable(vName,0);
    }

    public Variable SetVariable(string vName, float nValue)
    {

        Debug.Log("Set variable " + vName + " to " + nValue) ;
        if (EncodedVariables.ContainsKey(vName))
        {
            EncodedVariables[vName].SetFloatValue(nValue);
        }
        else
        {
            EncodedVariables.Add(vName, new Variable(nValue));
        }
        OnVariableChange(vName);
        return EncodedVariables[vName];
    }
    public Variable SetVariable(string vName, bool nValue)
    {
        if (EncodedVariables.ContainsKey(vName))
        {
            EncodedVariables[vName].SetBoolValue(nValue);
        }
        else
        {
            EncodedVariables.Add(vName, new Variable(nValue));
        }
        OnVariableChange(vName);
        return EncodedVariables[vName];
    }
    public Variable AddToVariable(string vName, float nValue)
    {
        if (EncodedVariables.ContainsKey(vName))
        {
            EncodedVariables[vName].SetFloatValue(EncodedVariables[vName].GetFloatValue() + nValue);
        }
        else
        {
            EncodedVariables.Add(vName, new Variable(nValue));
        }
        OnVariableChange(vName);
        return EncodedVariables[vName];
    }
    void OnVariableChange(string variable)
    {
        foreach (VariableReactionChange vrc in Reactors)
        {
            CheckVars(vrc, variable);
        }
    }
    public void UpdateReactors()
    {
        Reactors.RemoveAll((VariableReactionChange vrc) =>
        {
            return vrc == null;
        });
    }
    void CheckVars(VariableReactionChange vrc, string variable)
    {

        bool conditionMet = true;

        Debug.Log("REACTORCHEC " + vrc.name + "WITH " + vrc.Conditions.Length + " CONDITIONS");
        if (vrc.Conditions != null && vrc.Conditions.Length > 0)
        {
            bool tracksVariable = false;

            foreach (Condition c in vrc.Conditions)
            {
                Debug.Log("CONDITIONCHECK " + c.variableName + " NEEDS TO BE " + c.value);
                if (c.variableName == variable)
                    tracksVariable = true;
                if (!ConditionMet(c))
                {
                    Debug.Log("CONDITIONCHECK FAILED " + c.variableName);
                    conditionMet = false;
                    break;
                }
                Debug.Log("CONDITIONCHECK PASS " + c.variableName);
            }
            if (tracksVariable && conditionMet)
            {
                vrc.Action.Invoke();
            }
        }
    }
    public void Apply(VariableManager temp)
    {
        if (temp == null)
            return;
        foreach (KeyValuePair<string, Variable> num in temp.EncodedVariables)
        {
            SetVariable(num.Key, num.Value.GetFloatValue());
        }
    }
    public class Variable
    {
        float Value;
        public Variable(float value)
        {
            SetFloatValue(value);
        }
        public Variable(bool value)
        {
            SetBoolValue(value);
        }
        public void SetBoolValue(bool nValue)
        {
            Value = nValue ? 1 : 0;
        }
        public bool GetBoolValue()
        {
            return Value > 0;
        }
        public void SetFloatValue(float nValue)
        {
            Value = nValue;
        }
        public float GetFloatValue()
        {
            return Value;
        }
    }
    #region Set Variables
    [System.Serializable]
    public class Set
    {
        public enum Case
        {
            set,
            add,
            substract,
            min,
            max,
            multiply,
            divide
        }
        public string variableName;
        public float value;
        public Case change;
    }
    public void Apply(Set change)
    {
        float original = GetVariable(change.variableName).GetFloatValue();
        switch (change.change)
        {
            case Set.Case.set:
                SetVariable(change.variableName, change.value);
                break;
            case Set.Case.add:
                SetVariable(change.variableName, original + change.value);
                break;
            case Set.Case.substract:
                SetVariable(change.variableName, original - change.value);
                break;
            case Set.Case.min:
                SetVariable(change.variableName, Mathf.Min(original,change.value));
                break;
            case Set.Case.max:
                SetVariable(change.variableName, Mathf.Max(original, change.value));
                break;
            case Set.Case.multiply:
                SetVariable(change.variableName, original * change.value);
                break;
            case Set.Case.divide:
                SetVariable(change.variableName, original / change.value);
                break;
        }
    }
    public void Apply(Set[] changes)
    {
        if (changes == null || changes.Length == 0)
            return;
        foreach (Set num in changes)
        {
            Apply(num);
        }
    }
    #endregion
    #region Conditions
    [System.Serializable]
    public class Condition
    {
        public enum Check
        {
            equal,
            less,
            lesseq,
            greater,
            greatereg,
            boolean,
        }
        public string variableName;
        public float value;
        public Check change;
    }
    public bool ConditionMet(Condition c)
    {
        float original = GetVariable(c.variableName).GetFloatValue();
        switch (c.change)
        {
            case Condition.Check.equal:
                return original == c.value;
            case Condition.Check.boolean:
                return original == 1;
            case Condition.Check.less:
                return original < c.value;
            case Condition.Check.lesseq:
                return original <= c.value;
            case Condition.Check.greater:
                return original > c.value;
            case Condition.Check.greatereg:
                return original >= c.value;
        }
        return true;
    }
    public bool AllConditionsMet(Condition[] cs)
    {
        if (cs!=null && cs.Length > 0)
        foreach (Condition c in cs)
        {
            if (!ConditionMet(c))
                return false;
        }
        return true;
    }
    #endregion
}
