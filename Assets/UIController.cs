using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController main { get; private set; }
    [Header("Idea UI")]
    public IdeaButtonManager ShowIdeaWindowButton;
    public IdeaManager IdeaManagerWindow;
    public DialogueUIController dialogueController;

    private void Awake()
    {
        main = this;
        if (IdeaManagerWindow == null)
        {
            IdeaManagerWindow = GetComponentInChildren<IdeaManager>();
        }
        if (ShowIdeaWindowButton == null)
        {
            ShowIdeaWindowButton = GetComponentInChildren<IdeaButtonManager>();
        }
        if (dialogueController == null)
        {
            dialogueController = GetComponentInChildren<DialogueUIController>();
        }
    }
    private void Start()
    {
        dialogueController.gameObject.SetActive(false);
        IdeaManagerWindow.gameObject.SetActive(false);
    }
}
