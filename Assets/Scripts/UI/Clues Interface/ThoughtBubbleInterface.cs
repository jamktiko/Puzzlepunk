using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ThoughtBubbleInterface : MonoBehaviour
{
    public ThoughtContainer[] Clues;
    private void Awake()
    {
        Clues = GetComponentsInChildren<ThoughtContainer>();
        if (PuzzleBar == null)
        {
            PuzzleBar = GetComponentInChildren<ClueCombinerTopBar>();
            PuzzleBar.gameObject.SetActive(false);
        }       
    }
    public void OnNewClueRevealed()
    {
        foreach (ThoughtContainer clue in Clues)
        {
            clue.UpdateProgress();
        }
        UIController.main.ShowIdeaWindowButton.ShowNewIdea(PlayerClueController.main.HasNewClues);
    }

    public ClueCombinerTopBar PuzzleBar;
    public ClueChoiceSO Selection;
    private void OnEnable()
    {
        UpdatePuzzleUI();
    }
    private void OnDisable()
    {
        ClearPuzzleUI();
    }
    void UpdatePuzzleUI()
    {
        if (Selection != null)
        {
            PuzzleBar.Puzzle = Selection;
            PuzzleBar.gameObject.SetActive(true);
        }
        else
        {
            PuzzleBar.gameObject.SetActive(false);
        }
    }
    void ClearPuzzleUI()
    {
        Selection = null;
        PuzzleBar.gameObject.SetActive(false);
    }
}
