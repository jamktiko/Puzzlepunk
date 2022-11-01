using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RobotInputListener : MonoBehaviour
{
    void OnEnable()
    {
        PlayerInputListener.control.ZoePlayer.Movement.started += IssueOrder;

    }
    void OnDisable()
    {
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

            RobotNPC.WalkDirection order = RobotNPC.WalkDirection.empty;
            if (moveInput.y > 0)
            {
                order = RobotNPC.WalkDirection.up;
            }
            else if (moveInput.y < 0)
            {
                order = RobotNPC.WalkDirection.down;
            }
            else if (moveInput.x < 0)
            {
                order = RobotNPC.WalkDirection.left;
            }
            else if (moveInput.x > 0)
            {
                order = RobotNPC.WalkDirection.right;
            }
            if (order != RobotNPC.WalkDirection.empty)
                myRob.IssueOrder(order);
        }
    }
}
