using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class RobotNPC : MonoBehaviour
{
    public int CommandID = 0;
    public int MaxMoves = 1;
    public RobotPuzzleController puzzleParent;

    public MovementComponent movement;
    public GridNav.Node OriginalNode;

    private void Awake()
    {
        movement = GetComponent<MovementComponent>();
    }
    public void InitPuzzle(RobotPuzzleController parent)
    {
        puzzleParent = parent;
        movement.grid = parent.mGrid;
        OriginalNode = parent.mGrid.GetNodeAt(parent.mGrid.TranslateCoordinate(transform.position));
        OnReset(true);
    }
    public void OnReset(bool hard)
    {
        if (puzzleParent == null) return;
        cOrder = 0;
        movement.Stop();
        movement.JumpToPoint(OriginalNode);
        SetCrashed(false);
        if (hard)
            ClearOrders();
    }
    public Sprite GetSprite()
    {
        return GetComponent<SpriteRenderer>().sprite;
    }
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
    WalkDirection[] orders = new WalkDirection[10];
    public bool IssueOrder(WalkDirection order)
    {
        WalkDirection lastDir = WalkDirection.empty;
        for (int iO = 0; iO < orders.Length; iO++)
        {
            if (orders[iO] == WalkDirection.empty)
            {
                if (order != ReverseDirection(lastDir))
                {
                    orders[iO] = order;
                    UIController.main.robotController.orderMenu.UpdateOrders();//todo nullcheck
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
        for (int iO = 1; iO < orders.Length; iO++)
        {
            if (orders[iO] == WalkDirection.empty)
            {
                orders[iO - 1] = WalkDirection.empty;
                break;
            }
        }
    }
    public void ClearOrders()
    {
        cOrder = 0;
        for (int iO=0; iO<orders.Length; iO++)
        {
            orders[iO] = WalkDirection.empty;
        }
    }
    int cOrder = 0;
    public bool IsMoving()
    {
        return movement.IsWalking();
    }
    public bool IsIdle()
    {
        return cOrder < MaxMoves;
    }
    public void Step()
    {
        if (IsIdle())
        {
            movement.Stop();

            Vector2Int nPoint = Vector2Int.zero;
            switch (orders[cOrder])
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
            CheckCrash(movement.GetGridPosition() + nPoint);
            if (!HasCrashed())
                movement.MoveToPoint(movement.GetGridPosition() + nPoint);
            cOrder++;
        }

    }
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

        foreach (RobotNPC robot in puzzleParent.Robots)
        {
            if (robot != this && robot.movement.GetGridPosition() == node)
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
