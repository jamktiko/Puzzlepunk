using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RobotPuzzleController : PuzzleController
{
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
        OnReset(false);
    }

    public override void OnEnterPuzzle()
    {
        UIController.main.OpenWindow(UIController.UIWindow.robot);
        UIController.main.robotController.InitPuzzle(this);
        ChangeSelection(0);
        base.OnEnterPuzzle(); ;
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
    public void HandlePlayerOrders()
    {
        if (GetSelectedRobot() != null)
        {
            Vector2 moveInput = PlayerInputListener.control.ZoePlayer.Movement.ReadValue<Vector2>();

            RobotNPC.WalkDirection order = RobotNPC.WalkDirection.empty;
            if (moveInput.y > 0)
            {
                order = RobotNPC.WalkDirection.up;
            }
            else if (moveInput.y < 0)
            {
                order = RobotNPC.WalkDirection.down;
            }
            else if (moveInput.x < 0)
            {
                order = RobotNPC.WalkDirection.left;
            }
            else if (moveInput.x > 0)
            {
                order = RobotNPC.WalkDirection.right;
            }
            if (order!= RobotNPC.WalkDirection.empty)
                GetSelectedRobot().IssueOrder(order);
        }
    }
}
