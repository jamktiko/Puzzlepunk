using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController main { get; private set; }
    [Header("Idea UI")]
    public DialogueUIController dialogueController;

    public enum UIWindow
    {
        none,
        dialogue,
        brain
    }
    private void Awake()
    {
        main = this;
        if (dialogueController == null)
        {
            dialogueController = GetComponentInChildren<DialogueUIController>();
        }
        CloseWindow();
    }
    public void OpenWindow(UIWindow nWindow)
    {
        dialogueController.gameObject.SetActive(nWindow == UIWindow.dialogue);
    }
    public void CloseWindow()
    {
        OpenWindow( UIWindow.none);
        dialogueController.ForgetNPC();
    }
}
