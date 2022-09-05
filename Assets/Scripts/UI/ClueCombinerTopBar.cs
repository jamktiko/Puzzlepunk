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
    public Button ResetButton;
    List<string> cluesSelected = new List<string>();

    public void LoadChoices(ClueChoiceSO pzq)
    {
        Puzzle = pzq;
        HeaderText.text = pzq.DialogueQuestion;
        Clear();
    }
    internal void InsertClue(string text, string clue)
    {
        if (UIController.main.dialogueController.talkingNPC != null)
        {
            HandleNPCResponse(clue.ToLower());
        }
        else
        {
            if (ClueText.text == "" || cluesSelected.Count >= Puzzle.VariablesRequired.Length)
            {
                Clear();
                ClueText.text = text;
            }
            else
            {
                if (cluesSelected.Contains(clue.ToLower()))
                    return;
                ClueText.text = ClueText.text + " + " + text;
            }
            cluesSelected.Add(clue.ToLower());
            CheckSuccess();
        }
    }
    public void Clear()
    {
        ClueText.text = "";
        cluesSelected.Clear();
    }
    void CheckSuccess()
    {
        if (cluesSelected.Count != Puzzle.VariablesRequired.Length)
        {
            return;
        }
        foreach (string clue in Puzzle.VariablesRequired)
        {
            if (!cluesSelected.Contains(clue.ToLower()))
            {
                Debug.Log("Puzzle failed");
                return;
            }
        }

        if (Puzzle.SuccessDialogue != null)
        {
            Debug.Log("Puzzle complete");
            UIController.main.dialogueController.PlayCutscene(Puzzle.SuccessDialogue);
        }
    }
    #region NPCs
    void HandleNPCResponse(string variable)
    {
        foreach (CharacterSO.NPCReaction reply in UIController.main.dialogueController.talkingNPC.Reactions)
        {
            if (reply.VariableReaction.ToLower() == variable)
            {
                UIController.main.dialogueController.PlayCutscene(reply.ReactionDialogue);
                return;
            }
        }
        UIController.main.dialogueController.PlayCutscene(UIController.main.dialogueController.talkingNPC.StandardReply);
    }
    #endregion
}
