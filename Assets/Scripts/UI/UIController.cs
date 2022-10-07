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

        if (nWindow != UIWindow.none && CinematicsController.active!=null)
        {
            CinematicsController.active.SetPlayMode( CinematicsController.PlayMode.pause);
        }
    }
    public void CloseWindow()
    {
        OpenWindow(UIWindow.none);
        dialogueController.ForgetNPC();
        if (CinematicsController.active != null)
        {
            CinematicsController.active.SetPlayMode( CinematicsController.PlayMode.playing);
            CinematicsController.active = null;
        }
    }

}
