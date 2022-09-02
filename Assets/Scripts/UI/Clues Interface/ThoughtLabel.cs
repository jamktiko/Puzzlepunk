using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThoughtLabel : MonoBehaviour
{
    public enum ThoughtType
    {
        clue,
        important,
        conclusion
    }
    public TMPro.TextMeshProUGUI text;
    public string IdeaID = "-";
    public ThoughtType Thought = ThoughtType.important;

    private void Awake()
    {
        if (text == null)
            text = GetComponent<TMPro.TextMeshProUGUI>();
        SetRevealed(false);
    }
    public void SetRevealed(bool Value)
    {
        text.enabled = Value;
    }
}
