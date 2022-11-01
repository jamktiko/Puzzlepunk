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
        OriginalNode = parent.mGrid.GetNodeAt(parent.mGrid.TranslateCoordinate(transform.position));
    }
    public void OnReset(bool hard)
    {
        movement.Stop();
        transform.position = OriginalNode.worldPos;
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
    WalkDirection[] orders = new WalkDirection[10];
    public void IssueOrder(WalkDirection order)
    {
        for (int iO = 0; iO < orders.Length; iO++)
        {
            if (orders[iO] == WalkDirection.empty)
            {
                orders[iO] = order;
                UIController.main.robotController.orderMenu.UpdateOrders();//todo nullcheck

                return;
            }
        }
    }
    public WalkDirection GetOrderAt(int iO)
    {
        if (iO < orders.Length)
            return orders[iO];
        return WalkDirection.empty;
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
        return MoveCoroutine != null;
    }
    public bool IsIdle()
    {
        return cOrder <= orders.Length;
    }
    Coroutine MoveCoroutine;
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
            }
            movement.MoveToPoint(movement.GetGridPosition() + nPoint);
            cOrder++;
        }

    }
}
