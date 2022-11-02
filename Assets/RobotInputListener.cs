using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RobotInputListener : MonoBehaviour
{
    void OnEnable()
    {
        if (PlayerInputListener.control!=null)
            PlayerInputListener.control.ZoePlayer.Movement.started += IssueOrder;

    }
    void OnDisable()
    {
        if (PlayerInputListener.control != null)
            PlayerInputListener.control.ZoePlayer.Movement.started -= IssueOrder;
    }
    void IssueOrder(InputAction.CallbackContext context)
    {
        HandlePlayerOrders(context.ReadValue<Vector2>());
    }
    void HandlePlayerOrders(Vector2 moveInput)
    {
        RobotPuzzleController pzl = (RobotPuzzleController)PuzzleController.main;
        if (pzl == null) return;

        var myRob = pzl.GetSelectedRobot();

        if (myRob != null)
        {
            //Vector2 moveInput = .ReadValue<Vector2>();

            RobotPawn.WalkDirection order = RobotPawn.WalkDirection.empty;
            if (moveInput.y > 0)
            {
                order = RobotPawn.WalkDirection.up;
            }
            else if (moveInput.y < 0)
            {
                order = RobotPawn.WalkDirection.down;
            }
            else if (moveInput.x < 0)
            {
                order = RobotPawn.WalkDirection.left;
            }
            else if (moveInput.x > 0)
            {
                order = RobotPawn.WalkDirection.right;
            }
            if (order != RobotPawn.WalkDirection.empty)
                myRob.IssueOrder(order);
        }
    }
}
