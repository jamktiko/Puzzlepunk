using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class UIController : MonoBehaviour
{
    public static UIController main { get; private set; }
    [Header("Components")]
    public DialogueUIController dialogueController;
    public RobotMenu robotController;
    public TransitionScreenController TransitionScreen;


    public enum UIWindow
    {
        none,
        dialogue,
        robot
    }
    private void Awake()
    {
        main = this;
        if (dialogueController == null)
        {
            dialogueController = GetComponentInChildren<DialogueUIController>();
        }
        if (robotController == null)
        {
            robotController = GetComponentInChildren<RobotMenu>();
        }
        if (TransitionScreen == null)
        {
            TransitionScreen = GetComponentInChildren<TransitionScreenController>();
        }
        CloseWindow();
    }
    public void OpenWindow(UIWindow nWindow)
    {
        if (dialogueController!=null)
        dialogueController.gameObject.SetActive(nWindow == UIWindow.dialogue);

        if (robotController != null)
            robotController.gameObject.SetActive(nWindow == UIWindow.robot);

        if (nWindow != UIWindow.none && activeDirector != null && activeDirector.playableGraph.IsValid())
        {
            activeDirector.playableGraph.GetRootPlayable(0).SetSpeed(0);
        }
    }
    public void CloseWindow()
    {
        OpenWindow(UIWindow.none);
        dialogueController.ForgetNPC();
        if (activeDirector != null && activeDirector.playableGraph.IsValid())
        {
            activeDirector.playableGraph.GetRootPlayable(0).SetSpeed(1);
            activeDirector = null;
        }
    }


    public PlayableDirector activeDirector;
    public void AssignDirector(PlayableDirector nDirector)
    {
        activeDirector = nDirector;
    }
}
