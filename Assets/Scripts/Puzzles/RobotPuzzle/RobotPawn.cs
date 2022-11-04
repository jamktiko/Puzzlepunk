using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class RobotPawn : PuzzlePawn
{
    [Header("Configuration")]
    public int CommandID = 0;
    public int MaxMoves = 1;
    public bool OppositeOrders = false;

    [Header("Components")]

    public Sprite icon;
    public MovementComponent movement;
    public GridNav.Node OriginalNode;

    private void Awake()
    {
        movement = GetComponent<MovementComponent>();
    }
    public override void InitPuzzle(RobotPuzzleController parent)
    {
        movement.grid = parent.mGrid;
        movement.MovementSpeed = 1f / parent.StepTime;
        OriginalNode = parent.mGrid.GetNodeAt(parent.mGrid.TranslateCoordinate(transform.position));
        base.InitPuzzle(parent);
        InitOrders();
    }
    public override void OnReset(bool hard)
    {
        if (puzzleParent == null) return;
        cOrder = 0;
        movement.Stop();
        movement.JumpToPoint(OriginalNode);
        SetCrashed(false);
        if (hard)
            ClearOrders();
    }
    #region Orders
    public enum WalkDirection
    {
        empty = 0,
        up = 1,
        down = 2,
        left = 3,
        right = 4
    }
    public static WalkDirection ReverseDirection(WalkDirection dir)
    {
        switch (dir)
        {
            case WalkDirection.up:
                return WalkDirection.down;
            case WalkDirection.down:
                return WalkDirection.up;
            case WalkDirection.left:
                return WalkDirection.right;
            case WalkDirection.right:
                return WalkDirection.left;
        }
        return WalkDirection.empty;
    }
    public class Memory
    {
        public Sprite icon;
        public WalkDirection[] orders;
        public Memory(Sprite micon, int nOrders)
        {
            icon = micon;
            orders = new WalkDirection[nOrders];
        }
        public int iOrder = 0;
        public bool IssueOrder(WalkDirection order)
        {
            WalkDirection lastDir = WalkDirection.empty;
            for (int iO = 0; iO < orders.Length; iO++)
            {
                if (orders[iO] == WalkDirection.empty)
                {
                    if (order != ReverseDirection(lastDir))
                    {
                        iOrder++;
                        orders[iO] = order;
                        UIController.main.robotController.ResetOrderUI();//todo nullcheck
                        return true;
                    }
                }
                else
                {
                    lastDir = orders[iO];
                }
            }
            return false;
        }
        public WalkDirection GetOrderAt(int iO)
        {
            if (iO < orders.Length)
                return orders[iO];
            return WalkDirection.empty;
        }
        public void ClearLastOrder()
        {
            for (int iO = 1; iO <= orders.Length; iO++)
            {
                if (iO == orders.Length || orders[iO] == WalkDirection.empty)
                {
                    iOrder = Mathf.Max(0,iOrder-1);
                    orders[iO - 1] = WalkDirection.empty;
                    break;
                }
            }
        }
        public void ClearOrders()
        {
            iOrder = 0;
            for (int iO = 0; iO < orders.Length; iO++)
            {
                orders[iO] = WalkDirection.empty;
            }
        }
    }
    void InitOrders()
    {
        if (puzzleParent.RobotCommands.Count <= CommandID)
        {
            puzzleParent.RobotCommands.Add(new Memory(icon, MaxMoves));
        }
        else if (puzzleParent.RobotCommands[CommandID] == null)
            puzzleParent.RobotCommands[CommandID] = new Memory(icon, MaxMoves);
        
        memory = puzzleParent.RobotCommands[CommandID];

        puzzleParent.UpdateMoveLimit(MaxMoves);
    }
    Memory memory; //TODO nullchecks
    public void ClearOrders()
    {
        if (memory == null || memory.orders == null) return;
        cOrder = 0;
        memory.ClearOrders();
    }
    #endregion
    #region Moving Cycle
    int cOrder = 0;
    public bool IsMoving()
    {
        return movement.IsWalking();
    }
    public bool IsIdle()
    {
        return cOrder < MaxMoves;
    }
    Vector2Int GetNextStepDirection()
    {
        Vector2Int nPoint = Vector2Int.zero;

        WalkDirection walkDir = memory.orders[cOrder];
        if (OppositeOrders)
            walkDir = ReverseDirection(walkDir);

        switch (walkDir)
        {
            case WalkDirection.up:
                nPoint = Vector2Int.up;
                break;
            case WalkDirection.down:
                nPoint = Vector2Int.down;
                break;
            case WalkDirection.left:
                nPoint = Vector2Int.left;
                break;
            case WalkDirection.right:
                nPoint = Vector2Int.right;
                break;
            default:
                float randomValue = Random.value * 4;
                if (randomValue <= 1)
                {
                    nPoint = Vector2Int.right;
                }
                else if (randomValue <= 2)
                {
                    nPoint = Vector2Int.left;
                }
                else if (randomValue <= 3)
                {
                    nPoint = Vector2Int.up;
                }
                else if (randomValue <= 4)
                {
                    nPoint = Vector2Int.down;
                }
                break;
        }
        return nPoint;
    }
    public void Step()
    {
        if (IsIdle())
        {
            movement.Stop();
            Vector2Int nPoint = GetNextStepDirection();
            CheckCrash(movement.GetGridPosition() + nPoint);
            if (!HasCrashed())
                movement.MoveToPoint(movement.GetGridPosition() + nPoint);
            cOrder++;
        }

    }
    #endregion
    #region Crashing
    bool hasCrashed = false;
    void CheckCrash(Vector2Int node)
    {
        if (movement == null || movement.grid == null || puzzleParent == null)
            return;
        var nNode = movement.grid.GetNodeAt(node);
        if (nNode == null || !nNode.IsPassible())
        {
            SetCrashed(true);
            return;
        }

        foreach (RobotPawn robot in puzzleParent.Robots)
        {
            if (robot != this && robot.movement.GetGridPosition() + GetNextStepDirection() == node)
            {
                SetCrashed(true);
                return;
            }
        }
    }
    public bool HasCrashed()
    {
        return hasCrashed;
    }
    void SetCrashed(bool value)
    {
        if (value)
            Debug.Log(name + " HAS CRASHED! x.x");
        hasCrashed = value;
    }

    #endregion

}
