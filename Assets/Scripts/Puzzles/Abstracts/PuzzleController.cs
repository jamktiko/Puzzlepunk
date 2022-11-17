using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

public abstract class PuzzleController : MonoBehaviour
{
    public static PuzzleController main;
    protected bool solved = false;

    public bool CloseWithDialogue = false;
    public bool HardResetOnReenter = false;
    public bool ForceCinematic = true;

    public UnityEvent onSolve;

    public PuzzlePiece[] Pieces;
    private void Awake()
    {
        InitSolution();
    }
    protected virtual void InitSolution()
    {
        Pieces = GetComponentsInChildren<PuzzlePiece>();
        foreach (PuzzlePiece piece in Pieces)
        {
            piece.TieToPuzzle(this);
        }
        OnReset(true);
    }
    public void OnReset(bool hard)
    {
        if (solved) return;
        foreach (PuzzlePiece rob in Pieces)
        {
            rob.OnReset(hard);
        }
    }
    public void BeginPuzzle()
    {
        gameObject.SetActive(true);
        OnEnterPuzzle();
    }
    public void EndPuzzle()
    {
        TryShutDown();
    }
    protected virtual void OnEnterPuzzle()
    {
        main = this;
        OnReset(HardResetOnReenter);
        
        if (ForceCinematic)
            PlayerCinematicController.main.SetCinematicMode(true, false);
        UIController.main.CloseWindow(true);
    }
    protected virtual void OnExitPuzzle()
    {
        main = null;
        if (ForceCinematic)
            PlayerCinematicController.main.SetCinematicMode(false, false);
        UIController.main.CloseWindow(true);
    }
    public virtual bool TryShutDown()
    {
        if (UIController.main.dialogueController.IsInDialogueMode())
            return false;
        gameObject.SetActive(false);
        return true;
    }
    public virtual bool WasSolved()
    {
        return solved;
    }
    public virtual void CheckSolved()
    {
        if (!solved && WasSolved())
        {
            solved = true;
            onSolve.Invoke();
            Debug.Log("PUZZLE " + name.ToUpper() + " SOLVED!");
        }
    }
}
