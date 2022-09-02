using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThoughtLabel : MonoBehaviour
{
    public TMPro.TextMeshProUGUI text;
    public string IdeaID = "-";

    private void Awake()
    {
        if (text == null)
            text = GetComponent<TMPro.TextMeshProUGUI>();
        SetRevealed(false);
    }
    public void SetRevealed(bool Value)
    {
        text.text = Value ? IdeaID : "";
    }
}
