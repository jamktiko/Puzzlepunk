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
    private void Start()
    {
        foreach (ThoughtLabel ic in IdeaNames)
        {
            ic.gameObject.SetActive(ic.Revealed);
        }
        UpdateProgress();
    }
    public bool RevealClueByID(string clueID)
    {
        foreach (ThoughtLabel clue in IdeaNames)
        {
            if (clue.IdeaID == clueID)
            {
                clue.Reveal();
                UpdateProgress();
                return true;
            }
        }
        return false;
    }
    void UpdateProgress()
    {
        PercentComplete = 0;
        foreach (ThoughtLabel clue in IdeaNames)
        {
            if (clue.Revealed)
            {
                PercentComplete += 1 / IdeaNames.Length;
            }
        }
        gameObject.SetActive(PercentComplete > 0);
        conclusion.SetActive(PercentComplete >= PercentRequired);
    }
}
