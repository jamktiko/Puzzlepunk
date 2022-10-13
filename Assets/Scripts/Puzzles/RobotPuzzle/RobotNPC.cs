using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class RobotNPC : MonoBehaviour
{
    public float MovementSpeed = 10;
    public RobotPuzzleController puzzleParent;
    public GridNav.Node OriginalNode;    

    public void InitPuzzle(RobotPuzzleController parent)
    {
        puzzleParent = parent;
        OriginalNode = parent.mGrid.GetNodeAt(parent.mGrid.TranslateCoordinate(transform.position));
    }
    public void OnReset(bool hard)
    {
        EndStep();
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
        up,
        down,
        left,
        right
    }
    WalkDirection[] orders = new WalkDirection[10];
    public void IssueOrder(WalkDirection order)
    {
        for (int iO = 0; iO < orders.Length; iO++)
        {
            if (orders[iO] == WalkDirection.empty)
            {
                orders[iO] = order;
                return;
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
            EndStep();

            Vector3 nPoint = Vector3.zero;
            switch (orders[cOrder])
            {
                case WalkDirection.up:
                    nPoint = Vector3.up;
                    break;
                case WalkDirection.down:
                    nPoint = Vector3.down;
                    break;
                case WalkDirection.left:
                    nPoint = Vector3.left;
                    break;
                case WalkDirection.right:
                    nPoint = Vector3.right;
                    break;
            }
            MoveToPoint(transform.position + nPoint);
            cOrder++;
        }

    }
    GridNav.Node mNode;
    void MoveToPoint(Vector2 nPoint)
    {
        // Vector2Int point = grid.TranslateCoordinate( nPoint);
        mNode = puzzleParent.mGrid.GetNodeDirection(transform.position, nPoint - (Vector2)transform.position, 1);
        if (mNode == null || !mNode.IsPassible())
        {
            mNode = null;
        }
        if (mNode != null)
            MoveCoroutine = StartCoroutine(StepCoroutine());
    }
    IEnumerator StepCoroutine()
    {
        Vector3 moveDestination = transform.position;
        if (mNode != null)
            moveDestination = mNode.worldPos;

        while (mNode != null)
        {
            Vector3 delta = moveDestination - transform.position;
            if (delta.sqrMagnitude > MovementSpeed * MovementSpeed * Time.deltaTime * Time.deltaTime)
            {
                float speed = MovementSpeed * Time.deltaTime;
                transform.position += delta.normalized * speed;
                yield return new WaitForEndOfFrame();
            }
            else
            {
                transform.position = moveDestination;
                break;
            }
        }
        EndStep();
    }
    public void EndStep()
    {
        if (MoveCoroutine!=null)
        {
            StopCoroutine(MoveCoroutine);
            MoveCoroutine = null;
        }
    }
}
