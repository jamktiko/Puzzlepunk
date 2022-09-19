using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotNPC : MonoBehaviour
{
    public RobotPuzzleController puzzleParent;
    public GridNav.Node OriginalNode;

    public void InitPuzzle(RobotPuzzleController parent)
    {
        puzzleParent = parent;
        OriginalNode = parent.mGrid.GetNodeAt(parent.mGrid.TranslateCoordinate(transform.position));
    }
    public void Reset()
    {
        transform.position = OriginalNode.worldPos;
    }
}
