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
    public float GetSprintSpeed()
    {
        return (IsSprinting() ? SprintMultiplier : 1);
    }
    public GridNav grid;
    Vector2Int gridPos;
    public Vector2Int GetGridPosition()
    {
        return gridPos;
    }
    GridNav.Node mNode;
    public void MoveToWorldPoint(Vector2 nPoint)
    {
        if (grid == null)
            return;

        if (TryMoveWorldDirection(nPoint - (Vector2)transform.position, out GridNav.Node node))
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
    bool TryMoveWorldDirection(Vector2 direction, out GridNav.Node possible)
    {
        if (direction.x == 0 || direction.y == 0)
        {
            possible = grid.GetNodeDirection(transform.position, direction, MovementDistance);
            return possible != null;
        }
        else
        {
            if (grid.TryGetWorldNodeAt((Vector2)transform.position + direction * grid.UnitSize, out possible) && possible.IsPassible())
            {
                possible = grid.GetNodeDirection(transform.position, direction, MovementDistance);
                return possible != null;
            }
            else if (grid.TryGetWorldNodeAt((Vector2)transform.position + direction.x * Vector2.right * grid.UnitSize, out possible) && possible.IsPassible())
            {
                possible = grid.GetNodeDirection(transform.position, direction.x * Vector2.right, MovementDistance);
                return possible != null;
            }
            else if (grid.TryGetWorldNodeAt((Vector2)transform.position + direction.y * Vector2.up * grid.UnitSize, out possible) && possible.IsPassible())
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

        if (TryMoveDirection(nPoint - gridPos, out GridNav.Node node))
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
            return grid.TryGetNodeAt(gridPos + direction,out possible);
        }
        else
        {
            if (grid.TryGetNodeAt(gridPos + direction, out possible) && possible.IsPassible())
            {
                return possible != null;
            }
            else if (grid.TryGetWorldNodeAt(gridPos + direction.x * Vector2.right * grid.UnitSize, out possible) && possible.IsPassible())
            {
                return possible != null;
            }
            else if (grid.TryGetWorldNodeAt(gridPos + direction.y * Vector2.up * grid.UnitSize, out possible) && possible.IsPassible())
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
            float speed = MovementSpeed * Time.deltaTime * GetSprintSpeed();
            if (delta.sqrMagnitude > speed * speed)
            {
                transform.position += delta.normalized * speed;
                yield return new WaitForFixedUpdate();
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
