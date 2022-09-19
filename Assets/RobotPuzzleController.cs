using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RobotPuzzleController : MonoBehaviour
{
    bool WasSolved = false;
    public UnityEvent onSolve;
    public void RunPuzzle()
    {
        UIController.main.OpenWindow(UIController.UIWindow.robot);
        UIController.main.robotController.InitPuzzle(this);
    }
    public void ResetPuzzle()
    {
        
    }
}
