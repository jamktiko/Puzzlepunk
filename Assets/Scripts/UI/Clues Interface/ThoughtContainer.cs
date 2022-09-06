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
            bool Clue = PlayerClueController.main.GetClue(clue.IdeaID);
            clue.SetRevealed(Clue);
            if (Clue)
            {
                PercentComplete += 1f / IdeaNames.Length;
            }
        }
        Debug.Log(PercentComplete);
        gameObject.SetActive(PercentComplete > 0);
        conclusion.SetActive(PercentComplete >= PercentRequired);
    }
}
