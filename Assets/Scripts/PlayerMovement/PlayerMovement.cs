using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Vector2 facing = Vector2.down;

    public GridNav grid;

    public static PlayerMovement main;
    public float MovementSpeed = 1f;
    public float MovementDistance = 1f;
    public LayerMask BlockerMask;
    private void Awake()
    {
        main = this;
    }
    private void Update()
    {
        if (!CanAct())
        {
            if (IsWalking())
                Stop();
            return;
        }
        if (IsWalking())
            return;
        HandleMovement();
    }
    public bool CanAct()
    {
        if (UIController.main != null && (UIController.main.dialogueController.IsInDialogueMode() || UIController.main.robotController.IsPuzzleMode()))
            return false;

        if (PlayerCinematicController.main != null && PlayerCinematicController.main.IsInCinematicMode())
            return false;
        return true;
    }


    GridNav.Node mNode;
    void MoveToPoint(Vector2 nPoint)
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
    bool TryMoveDirection ( Vector2 direction, out GridNav.Node possible)
    {
        if (direction.x == 0 || direction.y == 0)
        {
            possible = grid.GetNodeDirection(transform.position, direction, MovementDistance);
                return possible!= null;
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

    public Vector2 moveInput = Vector2.zero;
    private void OnEnable()
    {
        PlayerInputListener.control.ZoePlayer.Movement.started += _ => { HandleMovement(); };
        PlayerInputListener.control.ZoePlayer.Movement.performed += _ => { HandleMovement(); };
        PlayerInputListener.control.ZoePlayer.Movement.canceled += _ => { HandleMovement(); };
    }
    void HandleMovement()
    {
        moveInput = PlayerInputListener.control.ZoePlayer.Movement.ReadValue<Vector2>();
        if (moveInput.x != 0  || moveInput.y != 0)
        {
            facing = moveInput;
            MoveToPoint((Vector2)transform.position + moveInput);
        }
        /*if (moveInput.x != 0)
        {
            MoveToPoint(transform.position + Vector3.right * moveInput.x );
            facing = moveInput.x > 0 ? Direction.right : Direction.left;
        }
        else if (moveInput.y != 0)
        {
            MoveToPoint(transform.position + Vector3.up * moveInput.y );
                facing = moveInput.y > 0 ? Direction.up : Direction.down;
        }*/
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
        PlayerInteractions.main.UpdateCanInteract();
        mNode = null;
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);
        moveCoroutine = null;
    }
    public bool IsWalking()
    {
        return mNode!=null && moveCoroutine != null;
    }
    public float SprintMultiplier = 2f;
    bool IsSprinting()
    {
        return PlayerInputListener.control.ZoePlayer.Skip.ReadValue<float>() > 0;
    }
    public void SetOrientation(Orientation nO)
    {
        Stop();
        switch (nO)
        {
            case Orientation.up:
                facing = Vector2.up;
                break;
            case Orientation.down:
                facing = Vector2.down;
                break;
            case Orientation.right:
                facing = Vector2.right;
                break;
            case Orientation.left:
                facing = Vector2.left;
                break;
            case Orientation.upleft:
                facing = Vector2.up + Vector2.left;
                break;
            case Orientation.downleft:
                facing = Vector2.down + Vector2.left;
                break;
            case Orientation.upright:
                facing = Vector2.down + Vector2.right;
                break;
            case Orientation.downright:
                facing = Vector2.down + Vector2.right;
                break;
        }
        PlayerAnimations.main.UpdateOrientation();
    }
    public enum Orientation
    {
        up,
        down,
        left,
        upleft,
        downleft,
        right,
        upright,
        downright
    }
}
