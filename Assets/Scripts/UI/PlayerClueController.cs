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
                PlayerClues.Add (clue.IdeaID.ToLower(), new Clue(clue, clue.Thought == ThoughtLabel.ThoughtType.conclusion, clue.Thought == ThoughtLabel.ThoughtType.important));
            }
        }
    }
    public bool HasNewClues = false;

    public Dictionary<string, Clue> PlayerClues;

    public void RevealClue(string clueID, bool value)
    {
        if (TryGetClue(clueID.ToLower(), out Clue cl))
        {
            bool update = cl.revealed != value;
            cl.revealed = value;
            if (update)
            {
                if (cl.revealed)
                {
                    HasNewClues = true;
                    UIController.main.flyingTextController.RevealClue(cl.GetName(), Input.mousePosition);
                }
                UIController.main.IdeaManagerWindow.OnNewClueRevealed();
            }
        }
    }
    public bool IsClueRevealed(string clueID)
    {
        Clue c = GetClue(clueID.ToLower());
        if (c == null)
        { return false; }
        return c.revealed;
    }
    public Clue GetClue(string clueID)
    {
        if (PlayerClues == null || !PlayerClues.ContainsKey(clueID.ToLower()))
        { return null; }
        return PlayerClues[clueID.ToLower()];
    }
    public bool TryGetClue(string clueID, out Clue variable)
    {
        variable = null;
        if (PlayerClues == null || !PlayerClues.ContainsKey(clueID.ToLower()))
        { return false; }
        variable = PlayerClues[clueID.ToLower()];
        return true;
    }
    public class Clue
    {
        public ThoughtLabel ThoughtLabel;
        public bool revealed;
        public bool important;

        public Clue(ThoughtLabel l, bool revealed, bool important)
        {
            ThoughtLabel = l;
            this.revealed = revealed;
            this.important = important;
        }
        public string GetName()
        {
            return ThoughtLabel.TextComponent.text;
        }
        public string GetID()
        {
            return ThoughtLabel.IdeaID.ToLower();
        }
    }
}
