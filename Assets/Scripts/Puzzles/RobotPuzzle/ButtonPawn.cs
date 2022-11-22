using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPawn : PuzzlePiece
{
    public int RequiresCommandID = -1;
    public Color ColorHighlighted = Color.white;
    public Color ColorPressed = Color.black;

    RobotPawn myRobot;

    public override void TieToPuzzle(PuzzleController parent)
    {
        base.TieToPuzzle(parent);
        RobotPuzzleController rpc = (RobotPuzzleController)parent;
        GridNav.Node posNode = rpc.mGrid.GetNodeAt(rpc.mGrid.TranslateCoordinate(transform.position));
        transform.position = posNode.worldPos;
    }
    public override void OnReset(bool hard)
    {
        SetPressed(false);
        base.OnReset(hard);
    }
    bool isPressed = false;
    public bool IsPressed()
    {
        return isPressed;
    }
    void SetPressed(bool value)
    {
        isPressed = value;
        GetComponent<SpriteRenderer>().color = isPressed ? ColorPressed : ColorHighlighted;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out RobotPawn robot))
        {
            myRobot = robot;
            if (RequiresCommandID < 0 || RequiresCommandID == robot.CommandID)
            SetPressed( true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out RobotPawn robot))
        {
            if (robot == myRobot)
            {
                myRobot = null;
                SetPressed(false);
            }
        }
    }
}
