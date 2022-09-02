using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClueController : MonoBehaviour
{
    public static PlayerClueController main;
    private void Awake()
    {
        main = this;
    }
    private void Start()
    {
        InitClues();
    }

    void InitClues()
    {
        PlayerClues = new Dictionary<string, bool>();
        foreach (ThoughtLabel clue in GetComponentsInChildren<ThoughtLabel>())
        {
            if (clue.IdeaID != "" && clue.IdeaID!= "-")
            {
                SetClue(clue.IdeaID, false);
            }
        }
    }
    public bool HasNewClues = false;

    public Dictionary<string, bool> PlayerClues;
    public void SetClue(string clueID, bool value)
    {
        if (!PlayerClues.ContainsKey(clueID))
        {
            PlayerClues.Add(clueID, value);
            if (value)
            {
                HasNewClues = true;
            }
        }
        else
        {
            if (!PlayerClues[clueID] && value)
            {
                HasNewClues = true;
            }
            PlayerClues[clueID] = value;
        }
            UIController.main.IdeaManagerWindow.OnNewClueRevealed();
        

    }
    public bool GetClue(string clueID)
    {
        if (PlayerClues == null || !PlayerClues.ContainsKey(clueID))
        { return false; }
        return PlayerClues[clueID];
    }
}
