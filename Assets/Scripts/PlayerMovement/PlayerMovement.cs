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
    public Vector2 facing = Vector2.down;

    public GridNav grid;

    public static PlayerMovement main;
    public float MovementSpeed = 1f;
    public float MovementDistance = 1f;
    public float InteractDistance = 1f;
    private void Awake()
    {
        main = this;
    }
    private void Update()
    {
        if ((UIController.main != null && (UIController.main.dialogueController.IsInDialogueMode() || UIController.main.robotController.IsPuzzleMode())) || (PlayerCinematicController.main!= null && PlayerCinematicController.main.IsInCinematicMode()))
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
        if (grid == null)
            return;

        if (TryMoveDirection(nPoint - (Vector2)transform.position, out mNode))
        {
            if (mNode.passible)
                moveCoroutine = StartCoroutine(Move());
            else
                mNode = null;
        }
    }
    bool TryMoveDirection ( Vector2 direction, out GridNav.Node possible)
    {
        possible = null;
        if (direction.x == 0 || direction.y == 0)
        {
            possible = grid.GetNodeDirection(transform.position, direction, MovementDistance);
            return possible!=null;
        }
        else 
        {
            if (grid.GetNodeAt((Vector2)transform.position + direction * grid.UnitSize, out possible) && possible.passible)
                {
                possible = grid.GetNodeDirection(transform.position, direction, MovementDistance);
                return possible != null;
            }
            else if (grid.GetNodeAt((Vector2)transform.position + direction.x * Vector2.right * grid.UnitSize, out possible) && possible.passible)
            {
                possible = grid.GetNodeDirection(transform.position, direction.x * Vector2.right, MovementDistance);
                return possible != null;
            }
            else if (grid.GetNodeAt((Vector2)transform.position + direction.y * Vector2.up * grid.UnitSize, out possible) && possible.passible)
            {
                possible = grid.GetNodeDirection(transform.position, direction.y * Vector2.up, MovementDistance);
                return possible != null;
            }

            return false;
        }
    }

    public Vector2 moveInput = Vector2.zero;
    void HandleMovement()
    {
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
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
        mNode = null;
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);
        moveCoroutine = null;
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
    public void PlayerInteract()
    {
        Vector2 point = (Vector2)transform.position + facing * InteractDistance;
        foreach (RaycastHit2D hit in Physics2D.CircleCastAll(transform.position, .1f, facing , InteractDistance))
        {
            if (hit.transform.TryGetComponent(out InteractableBase interactable))
            {
                
                    interactable.OnInteract();
                return;
            }
        }
    }
}
