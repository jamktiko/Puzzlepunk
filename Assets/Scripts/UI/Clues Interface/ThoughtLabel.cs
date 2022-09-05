using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThoughtLabel : MonoBehaviour
{
    public enum ThoughtType
    {
        clue,
        important,
        conclusion
    }
    public TMPro.TextMeshProUGUI text;
    public Button btn;
    public string IdeaID = "-";
    public ThoughtType Thought = ThoughtType.important;

    private void Awake()
    {
        if (text == null)
            text = GetComponent<TMPro.TextMeshProUGUI>();
        InitButton();
        SetRevealed(false);
    }
    public void SetRevealed(bool Value)
    {
        text.enabled = Value;
    }
    void InitButton()
    {
        if (btn == null)
            btn = GetComponent<Button>();
        btn.onClick.AddListener(() =>
        {
            this.OnButtonClick();
        });
    }
    void OnButtonClick()
    {
        var puzzleBar = UIController.main.IdeaManagerWindow.PuzzleBar;
        if (puzzleBar.Puzzle != null || puzzleBar.talkingNPC)
        {
            puzzleBar.InsertClue(text.text, IdeaID);
        }
    }
}
