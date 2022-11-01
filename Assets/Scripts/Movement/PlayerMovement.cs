using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Vector2 facing = Vector2.down;


    public static PlayerMovement main;
    public LayerMask BlockerMask;
    public MovementComponent movement;
    private void Awake()
    {
        main = this;
        if (movement == null)
            movement = GetComponent<MovementComponent>();
    }
    private void Start()
    {
        if (PlayerInputListener.control != null)
        {
            PlayerInputListener.control.ZoePlayer.Skip.started += _ => { movement.SetSpriting( true); };
            PlayerInputListener.control.ZoePlayer.Skip.canceled += _ => { movement.SetSpriting( false); };
        }
    }
    private void Update()
    {
        if (!CanAct())
        {
            if (movement.IsWalking())
                movement.Stop();
            return;
        }
        if (movement.IsWalking())
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



    public Vector2 moveInput = Vector2.zero;
    void HandleMovement()
    {
        moveInput = PlayerInputListener.control.ZoePlayer.Movement.ReadValue<Vector2>();
        if (moveInput.x != 0  || moveInput.y != 0)
        {
            facing = moveInput;
            movement.MoveToPoint((Vector2)transform.position + moveInput);
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
    public void SetOrientation(Orientation nO)
    {
        movement.Stop();
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
}
