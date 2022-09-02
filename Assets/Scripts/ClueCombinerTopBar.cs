using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClueCombinerTopBar : MonoBehaviour
{
    public ClueChoiceSO Puzzle;
    public TextMeshProUGUI HeaderText;
    public TextMeshProUGUI ClueText;
    public Button  ResetButton;
    List<string> cluesSelected = new List<string>();

    public void LoadChoices(ClueChoiceSO pzq)
    {
        Puzzle = pzq;
        Clear();
    }
    internal void InsertClue(string text, string clue)
    {
        if (ClueText.text == "" || cluesSelected.Count >= Puzzle.DialogueQuestion.Length)
        {
            Clear();
            ClueText.text = text;
        }
        else
        {
            ClueText.text = ClueText.text + " + " + text;
        }
        cluesSelected .Add( clue);
    }
    public void Clear()
    {
        ClueText.text = "";
        cluesSelected.Clear();
    }
}
