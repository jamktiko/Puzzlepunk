using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThoughtContainer : MonoBehaviour
{
    float PercentComplete = 0;
    public float PercentRequired = .5f;
    public ThoughtLabel[] IdeaNames;
    public GameObject conclusion;
    private void Awake()
    {
        IdeaNames = GetComponentsInChildren<ThoughtLabel>();
        if (conclusion==null)
        {
            conclusion = transform.GetChild(1).gameObject;
        }
    }
    public void UpdateProgress()
    {
        PercentComplete = 0;
        foreach (ThoughtLabel clue in IdeaNames)
        {
            if (PlayerClueController.main.TryGetClue(clue.IdeaID, out PlayerClueController.Clue c)) {
                
                clue.SetRevealed(c.revealed);
                if (c.revealed && c.important)
                {
                    PercentComplete += 1f / IdeaNames.Length;
                }
            }
            else
            {
                clue.SetRevealed(false);
            }
        }
        gameObject.SetActive(PercentComplete > 0);

        if (PercentComplete > 0)
        {
            conclusion.SetActive(PercentComplete >= PercentRequired);
        }
    }
}
