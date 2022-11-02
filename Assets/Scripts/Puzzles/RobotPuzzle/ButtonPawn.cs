using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPawn : PuzzlePawn
{
    public int RequiresCommandID = -1;
    public override void InitPuzzle(RobotPuzzleController parent)
    {
        base.InitPuzzle(parent);
        GridNav.Node posNode = parent.mGrid.GetNodeAt(parent.mGrid.TranslateCoordinate(transform.position));
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
        GetComponent<SpriteRenderer>().color = isPressed ? Color.cyan : Color.blue;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out RobotPawn robot))
        {
            if (RequiresCommandID == -1 || RequiresCommandID == robot.CommandID)
            SetPressed( true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out RobotPawn robot))
        {
            SetPressed( false);
        }
    }
}
