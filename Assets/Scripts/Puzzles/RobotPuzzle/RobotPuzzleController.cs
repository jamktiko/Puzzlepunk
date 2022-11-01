using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RobotPuzzleController : PuzzleController
{
    public RobotNPC[] Robots;
    public GridNav mGrid;
    public CameraBounds cambounds;
    private void Start()
    {
        Init();
    }
    private void Init()
    {
        mGrid = GetComponentInChildren<GridNav>();

        Robots = GetComponentsInChildren<RobotNPC>();//Todo nullchecks
        foreach (RobotNPC rob in Robots)
        {
            rob.InitPuzzle( this);
        }
        OnReset(false);
    }

    public override void OnEnterPuzzle()
    {
        base.OnEnterPuzzle();
        UIController.main.OpenWindow(UIController.UIWindow.robot);
        UIController.main.robotController.InitPuzzle(this);
        ChangeSelection(0);
        if (cambounds!=null)
        CameraController.main.SetBounds(cambounds);

    }
    public void ChangeSelection(int sel)
    {
        Selection = sel;
        UIController.main.robotController.OnSelectionChanged();
    }
    public void OnReset(bool hard)
    {
        foreach (RobotNPC rob in Robots)
        {
            rob.OnReset(hard);
        }
    }
    public int Selection = 0;
    public RobotNPC GetSelectedRobot()
    {
        return Robots[Selection];
    }
}
