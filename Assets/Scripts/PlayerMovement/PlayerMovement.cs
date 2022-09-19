using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public enum Direction
    {
        up,
        down,
        left,
        right
    }
    public Direction facing = Direction.down;

    public GridNav grid;

    public static PlayerMovement main;
    public float MovementSpeed = 1f;
    private void Awake()
    {
        main = this;
    }
    private void Update()
    {
        if ((UIController.main != null && UIController.main.dialogueController.IsInDialogueMode()) || PlayerCinematicController.main.IsInCinematicMode())
            return;
        if (IsWalking())
            return;
        HandleMovement();
        HandleInteractCommand();
    }
    public void HandleInteractCommand()
    {
        if (Input.GetKeyDown(KeyCode.X))
            PlayerInteract();
    }

    GridNav.Node mNode;
    void MoveToPoint(Vector2 nPoint)
    {
       // Vector2Int point = grid.TranslateCoordinate( nPoint);
        mNode = grid.GetNodeDirection(transform.position, nPoint-(Vector2)transform.position, 1);
        if (mNode == null || !mNode.passible)
        {
            mNode = null;
        }
        if (mNode != null)
            moveCoroutine = StartCoroutine(Move());
    }
    void HandleMovement()
    {
        Vector2 iV = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (iV.x != 0)
        {
            MoveToPoint(transform.position + Vector3.right * iV.x);
            facing = iV.x > 0 ? Direction.right : Direction.left;
        }
        else if (iV.y != 0)
        {
            MoveToPoint(transform.position + Vector3.up * iV.y);
            facing = iV.y > 0 ? Direction.up : Direction.down;
        }
    }
    Coroutine moveCoroutine;
    IEnumerator Move()
    {
        Vector3 moveDestination = transform.position;
        if (mNode!=null)
            moveDestination = mNode.worldPos;

        while (mNode != null)
        {
            Vector3 delta = moveDestination - transform.position;
            if (delta.sqrMagnitude > MovementSpeed * MovementSpeed * Time.deltaTime * Time.deltaTime)
            {
                float speed = MovementSpeed * Time.deltaTime * (IsSprinting() ? SprintMultiplier : 1);
                transform.position += delta.normalized * speed;
            }
            else
            {
                transform.position = moveDestination;
                Stop();
            }
            yield return new WaitForEndOfFrame();
        }
    }
    public void Stop()
    {
        mNode = null;
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);
        moveCoroutine = null;
    }
    public bool IsFacingRight()
    {
        return facing == Direction.right;
    }
    public bool IsWalking()
    {
        return mNode!=null &&  moveCoroutine != null;
    }
    public float SprintMultiplier = 2f;
    bool IsSprinting()
    {
        return Input.GetMouseButton(0);
    }
    public Vector3 GetForwardVector()
    {
        switch (facing)
        {
            case Direction.up:
                return Vector3.up;
            case Direction.down:
                return Vector3.down;
            case Direction.left:
                return Vector3.left;
            case Direction.right:
                return Vector3.right;
        }
        return Vector3.down;
    }
    public void PlayerInteract()
    {
        Vector2 point = transform.position + GetForwardVector();
        foreach (RaycastHit2D hit in Physics2D.CircleCastAll(point, .1f, Vector2.zero))
        {
            if (hit.transform.TryGetComponent(out InteractableBase interactable))
            {
                
                    interactable.OnInteract();
            }
        }
    }
}
