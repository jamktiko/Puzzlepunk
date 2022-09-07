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
    public TMPro.TextMeshProUGUI TextComponent;
    public Button btn;
    public string IdeaID = "-";
    public ThoughtType Thought = ThoughtType.important;

    public Color enabledColor = Color.black;
    public Color disabledColor = Color.clear;

    private void Awake()
    {
        if (TextComponent == null)
            TextComponent = GetComponent<TMPro.TextMeshProUGUI>();
        InitButton();
        SetRevealed(false);
    }
    public void SetRevealed(bool Value)
    {
        TextComponent.color = Value ? enabledColor : disabledColor;
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
        if (puzzleBar.Puzzle != null || UIController.main.dialogueController.talkingNPC!=null)
        {
            puzzleBar.InsertClue(TextComponent.text, IdeaID);
        }
    }
}
