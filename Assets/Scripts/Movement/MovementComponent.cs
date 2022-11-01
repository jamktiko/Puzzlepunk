using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementComponent : MonoBehaviour
{
    public float SprintMultiplier = 2f;
    public float MovementSpeed = 1f;
    public float MovementDistance = 1f;
    bool Sprinting = false;
    public void SetSpriting(bool value)
    {
        Sprinting = value;
    }
    bool IsSprinting()
    {
        return Sprinting;
    }
    public GridNav grid;
    Vector2Int gridPos;
    public Vector2Int GetGridPosition()
    {
        return gridPos;
    }
    GridNav.Node mNode;
    public void MoveToPoint(Vector2 nPoint)
    {
        if (grid == null)
            return;

        if (TryMoveDirection(nPoint - (Vector2)transform.position, out GridNav.Node node))
        {
            if (node.IsPassible())
            {
                mNode = node;
                moveCoroutine = StartCoroutine(Move());
            }
            else
            {
                mNode = null;
            }
        }
    }
    bool TryMoveDirection(Vector2 direction, out GridNav.Node possible)
    {
        if (direction.x == 0 || direction.y == 0)
        {
            possible = grid.GetNodeDirection(transform.position, direction, MovementDistance);
            return possible != null;
        }
        else
        {
            if (grid.GetNodeAt((Vector2)transform.position + direction * grid.UnitSize, out possible) && possible.IsPassible())
            {
                possible = grid.GetNodeDirection(transform.position, direction, MovementDistance);
                return possible != null;
            }
            else if (grid.GetNodeAt((Vector2)transform.position + direction.x * Vector2.right * grid.UnitSize, out possible) && possible.IsPassible())
            {
                possible = grid.GetNodeDirection(transform.position, direction.x * Vector2.right, MovementDistance);
                return possible != null;
            }
            else if (grid.GetNodeAt((Vector2)transform.position + direction.y * Vector2.up * grid.UnitSize, out possible) && possible.IsPassible())
            {
                possible = grid.GetNodeDirection(transform.position, direction.y * Vector2.up, MovementDistance);
                return possible != null;
            }

            return false;
        }
    }
    public void MoveToPoint(Vector2Int nPoint)
    {
        if (grid == null)
            return;

        if (TryMoveDirection(nPoint - (Vector2)transform.position, out GridNav.Node node))
        {
            if (node.IsPassible())
            {
                mNode = node;
                moveCoroutine = StartCoroutine(Move());
            }
            else
            {
                mNode = null;
            }
        }
    }
    bool TryMoveDirection(Vector2Int direction, out GridNav.Node possible)
    {
        if (direction.x == 0 || direction.y == 0)
        {
            return grid.GetNodeAt(gridPos + direction,out possible);
        }
        else
        {
            if (grid.GetNodeAt(gridPos + direction, out possible) && possible.IsPassible())
            {
                return possible != null;
            }
            else if (grid.GetNodeAt(gridPos + direction.x * Vector2.right * grid.UnitSize, out possible) && possible.IsPassible())
            {
                return possible != null;
            }
            else if (grid.GetNodeAt(gridPos + direction.y * Vector2.up * grid.UnitSize, out possible) && possible.IsPassible())
            {
                return possible != null;
            }

            return false;
        }
    }
    Coroutine moveCoroutine;
    IEnumerator Move()
    {
        Vector3 moveDestination = transform.position;
        if (mNode != null)
            moveDestination = mNode.worldPos;

        while (mNode != null)
        {
            Vector3 delta = moveDestination - transform.position;
            if (delta.sqrMagnitude > MovementSpeed * MovementSpeed * Time.deltaTime * Time.deltaTime)
            {
                float speed = MovementSpeed * Time.deltaTime * (IsSprinting() ? SprintMultiplier : 1);
                transform.position += delta.normalized * speed;
                yield return new WaitForEndOfFrame();
            }
            else
            {
                transform.position = moveDestination;
                break;
            }
        }
        Stop();
    }
    public void Stop()
    {
        PlayerInteractions.main.ClearInteractable();
        if (mNode!=null)
            gridPos = mNode.gridPos;
        mNode = null;
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);
        moveCoroutine = null;
    }
    public bool IsWalking()
    {
        return mNode != null && moveCoroutine != null;
    }
    public void JumpToPoint(GridNav.Node point)
    {
        gridPos = point.gridPos;
        transform.position = point.worldPos;
    }
}
