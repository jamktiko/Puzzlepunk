using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class PuzzleController : MonoBehaviour
{
    public bool ForceCinematic = true;
    public static PuzzleController main;
    protected bool WasSolved = false;
    public UnityEvent onSolve;

    public void BeginPuzzle()
    {
        OnEnterPuzzle();
    }
    public virtual void OnEnterPuzzle()
    {
        main = this;
        if (ForceCinematic)
            PlayerCinematicController.main.SetCinematicMode(true, false);
    }
    public virtual void OnExitPuzzle()
    {
        main = null;
        if (ForceCinematic)
            PlayerCinematicController.main.SetCinematicMode(false, false);
    }
}
