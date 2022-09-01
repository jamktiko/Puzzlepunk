using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThoughtBubbleInterface : MonoBehaviour
{
    public ThoughtContainer[] Clues;
    private void Awake()
    {
        Clues = GetComponentsInChildren<ThoughtContainer>();
    }
    public void RevealClue(string clueID)
    {
        foreach (ThoughtContainer clue in Clues)
        {
            if (clue.RevealClueByID(clueID))
            {
                UIController.main.ShowIdeaWindowButton.ShowNewIdea(true);
                return;
            }
        }
    }
}
