using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RobotPuzzleController : MonoBehaviour
{
    bool WasSolved = false;
    public UnityEvent onSolve;
    public RobotNPC[] Robots;
    public GridNav mGrid;
    private void Start()
    {
        Init();
    }
    private void Init()
    {
        mGrid = GetComponentInChildren<GridNav>();

        Robots = GetComponentsInChildren<RobotNPC>();
        foreach (RobotNPC rob in Robots)
        {
            rob.InitPuzzle( this);
        }
        Reset();
    }

    public void RunPuzzle()
    {
        UIController.main.OpenWindow(UIController.UIWindow.robot);
        UIController.main.robotController.InitPuzzle(this);
    }
    public void Reset()
    {
        foreach (RobotNPC rob in Robots)
        {
            rob.Reset();
        }
    }
}
