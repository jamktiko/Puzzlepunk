using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

public abstract class PuzzleController : MonoBehaviour
{
    public bool ForceCinematic = true;
    public static PuzzleController main;
    protected bool WasSolved = false;
    public bool AlwaysHardReset = false;
    public UnityEvent onSolve;

    public PuzzlePiece[] Pieces;
    private void Start()
    {
        InitSolution();
    }
    protected virtual void InitSolution()
    {
        Pieces = GetComponentsInChildren<PuzzlePiece>();
        foreach (PuzzlePiece rob in Pieces)
        {
            rob.TieToPuzzle(this);
        }
        OnReset(true);
    }
    public void OnReset(bool hard)
    {
        if (WasSolved) return;
        foreach (PuzzlePiece rob in Pieces)
        {
            rob.OnReset(hard);
        }
    }
    public void BeginPuzzle()
    {
        OnEnterPuzzle();
    }
    public virtual void OnEnterPuzzle()
    {
        main = this;
        OnReset(AlwaysHardReset);
        
        if (ForceCinematic)
            PlayerCinematicController.main.SetCinematicMode(true, false);
        
    }
    public virtual void OnExitPuzzle()
    {
        main = null;
        if (ForceCinematic)
            PlayerCinematicController.main.SetCinematicMode(false, false);
    }
    public virtual bool TrySolve()
    {
        return WasSolved;
    }
    public virtual void CheckSolved()
    {
        if (!WasSolved && TrySolve())
        {
            WasSolved = true;
            onSolve.Invoke();
            Debug.Log("PUZZLE " + name.ToUpper() + " SOLVED!");
        }
    }
}
