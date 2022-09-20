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
    private void Update()
    {
        HandlePlayerOrders();
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

    public void RunPuzzle()
    {
        UIController.main.OpenWindow(UIController.UIWindow.robot);
        UIController.main.robotController.InitPuzzle(this);
        ChangeSelection(0);
    }
    public void ChangeSelection(int sel)
    {
        Selection = sel;
        UIController.main.robotController.OnSelectionChanged();
    }
    public void OnReset(bool soft)
    {
        foreach (RobotNPC rob in Robots)
        {
            rob.OnReset(soft);
        }
    }
    public int Selection = 0;
    public RobotNPC GetSelectedRobot()
    {
        return Robots[Selection];
    }
    public void HandlePlayerOrders()
    {
        if (GetSelectedRobot() == null)
        {
            RobotNPC.WalkDirection order = RobotNPC.WalkDirection.empty;
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                order = RobotNPC.WalkDirection.up;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                order = RobotNPC.WalkDirection.down;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                order = RobotNPC.WalkDirection.left;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                order = RobotNPC.WalkDirection.right;
            }
            GetSelectedRobot().IssueOrder(order);
        }
    }
}
