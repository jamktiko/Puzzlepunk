using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PuzzlePiece : MonoBehaviour
{
    public PuzzleController puzzleParent;
    public virtual void TieToPuzzle(PuzzleController parent)
    {
        puzzleParent = parent;
        OnReset(true);
    }
    public virtual void OnReset(bool hard)
    {

    }
}
