using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController main { get; private set; }
    [Header("Idea UI")]
    public IdeaButtonManager ShowIdeaWindowButton;
    public ThoughtBubbleInterface IdeaManagerWindow;
    public DialogueUIController dialogueController;
    public FlyingTextUI flyingTextController;

    public enum UIWindow
    {
        none,
        dialogue,
        brain
    }
    private void Awake()
    {
        main = this;
        if (IdeaManagerWindow == null)
        {
            IdeaManagerWindow = GetComponentInChildren<ThoughtBubbleInterface>();
        }
        if (ShowIdeaWindowButton == null)
        {
            ShowIdeaWindowButton = GetComponentInChildren<IdeaButtonManager>();
        }
        if (dialogueController == null)
        {
            dialogueController = GetComponentInChildren<DialogueUIController>();
        }
        if (flyingTextController == null)
        {
            flyingTextController = GetComponentInChildren<FlyingTextUI>();
        }
        CloseWindow();
        flyingTextController.Hide();
    }
    public void OpenWindow(UIWindow nWindow)
    {
        dialogueController.gameObject.SetActive(nWindow == UIWindow.dialogue);
        IdeaManagerWindow.gameObject.SetActive(nWindow == UIWindow.brain);
    }
    public void CloseWindow()
    {
        OpenWindow( UIWindow.none);
        dialogueController.ForgetNPC();
        //IdeaManagerWindow.ClearNPC();
    }
}
