using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController main { get; private set; }
    [Header("Components")]
    public DialogueUIController dialogueController;
    public RobotMenu robotController;

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
        CloseWindow();
    }
    public void OpenWindow(UIWindow nWindow)
    {
        if (dialogueController!=null)
        dialogueController.gameObject.SetActive(nWindow == UIWindow.dialogue);

        if (robotController != null)
            robotController.gameObject.SetActive(nWindow == UIWindow.robot);
    }
    public void CloseWindow()
    {
        OpenWindow( UIWindow.none);
        dialogueController.ForgetNPC();
    }
}
