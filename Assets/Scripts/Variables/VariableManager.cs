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
            bool tracksVariable = false;
            bool conditionMet = true;

            if (vrc.Conditions != null && vrc.Conditions.Length > 0)
                foreach (Condition c in vrc.Conditions)
                {
                    if (c.variableName == variable)
                        tracksVariable = true;
                    if (!ConditionMet(c))
                    {
                        conditionMet = false;
                        break;
                    }
                }
            if (conditionMet && tracksVariable)
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
    [System.Serializable]
    public class Condition
    {
        public string variableName;
        public int value;
    }
    public bool ConditionMet(Condition c)
    {
        return GetVariable(c.variableName).GetFloatValue() == c.value;
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
}
