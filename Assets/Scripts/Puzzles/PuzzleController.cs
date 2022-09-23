using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class PuzzleController : MonoBehaviour
{
    public static PuzzleController main;
    protected bool WasSolved = false;
    public UnityEvent onSolve;

    public virtual void OnEnterPuzzle()
    {
        main = this;
    }
    public virtual void OnExitPuzzle()
    {
        main = null;
    }
}
