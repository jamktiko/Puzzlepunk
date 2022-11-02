using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePawn : MonoBehaviour
{
    public RobotPuzzleController puzzleParent;
    public virtual void InitPuzzle(RobotPuzzleController parent)
    {
        puzzleParent = parent;
        OnReset(true);
    }
    public virtual void OnReset(bool hard)
    {

    }
}
