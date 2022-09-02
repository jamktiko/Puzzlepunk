using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClueController : MonoBehaviour
{
    public static PlayerClueController main;
    private void Awake()
    {
        main = this;
        InitClues();
    }

    void InitClues()
    {
        PlayerClues = new Dictionary<string, Clue>();
        foreach (ThoughtLabel clue in GetComponentsInChildren<ThoughtLabel>())
        {
            if (clue.IdeaID != "" && clue.IdeaID!= "-")
            {
                PlayerClues.Add (clue.IdeaID, new Clue(clue.IdeaID, clue.Thought == ThoughtLabel.ThoughtType.conclusion, clue.Thought == ThoughtLabel.ThoughtType.important));
            }
        }
    }
    public bool HasNewClues = false;

    public Dictionary<string, Clue> PlayerClues;

    public void RevealClue(string clueID, bool value)
    {
        if (TryGetClue(clueID,out Clue cl))
        {
            bool update = cl.revealed != value;
            cl.revealed = value;
            if (update)
            {
                if (cl.revealed)
                {
                    HasNewClues = true;
                }
                UIController.main.IdeaManagerWindow.OnNewClueRevealed();
            }
        }
    }
    public bool IsClueRevealed(string clueID)
    {
        Clue c = GetClue(clueID);
        if (c == null)
        { return false; }
        return c.revealed;
    }
    public Clue GetClue(string clueID)
    {
        if (PlayerClues == null || !PlayerClues.ContainsKey(clueID))
        { return null; }
        return PlayerClues[clueID];
    }
    public bool TryGetClue(string clueID, out Clue variable)
    {
        variable = null;
        if (PlayerClues == null || !PlayerClues.ContainsKey(clueID))
        { return false; }
        variable = PlayerClues[clueID];
        return true;
    }
    public class Clue
    {
        public string ID;
        public bool revealed;
        public bool important;

        public Clue(string iD, bool revealed, bool important)
        {
            ID = iD;
            this.revealed = revealed;
            this.important = important;
        }
    }
}
