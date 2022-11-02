using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RobotPuzzleController : PuzzleController
{
    public CameraBounds cambounds;
    public GridNav mGrid;

    public PuzzlePawn[] Pieces;      
    public ButtonPawn[] Objectives;

    public RobotPawn[] Robots;
    public List<RobotPawn.Memory> RobotCommands;

    private void Awake()
    {
        RobotCommands = new List<RobotPawn.Memory>();   
    }
    private void Start()
    {
        Init();
    }
    private void Init()
    {
        mGrid = GetComponentInChildren<GridNav>();

        Pieces = GetComponentsInChildren<PuzzlePawn>();
        foreach (PuzzlePawn rob in Pieces)
        {
            rob.InitPuzzle( this);
        }

        Objectives = GetComponentsInChildren<ButtonPawn>();
        Robots = GetComponentsInChildren<RobotPawn>();//Todo nullchecks

        OnReset(true);
    }

    public override void OnEnterPuzzle()
    {
        base.OnEnterPuzzle();
        UIController.main.OpenWindow(UIController.UIWindow.robot);
        UIController.main.robotController.InitPuzzle(this);
        ChangeSelection(0);
        if (cambounds!=null)
        CameraController.main.SetBounds(cambounds);

    }
    public override void OnExitPuzzle()
    {
        base.OnExitPuzzle();
        EndPuzzle();
        if (cambounds != null)
            CameraController.main.SetBounds(PlayerTransitionController.main.CurrentRoom.bounds);
    }
    #region Selection
    public int Selection = 0;
    public void ChangeSelection(int sel)
    {
        Selection = sel;
        UIController.main.robotController.OnSelectionChanged();
    }
    public RobotPawn.Memory GetSelectedRobot()
    {
        return RobotCommands[Selection];
    }
    #endregion
    public void OnReset(bool hard)
    {
        if (WasSolved) return;
        foreach (PuzzlePawn rob in Pieces)
        {
            rob.OnReset(hard);
        }
    }
    #region Play
    bool PuzzleFailed = false;
    public void PlaySolution()
    {
        EndPuzzle();
        PlayCoroutine = StartCoroutine(PuzzleCoroutine());
    }
    Coroutine PlayCoroutine;
    IEnumerator PuzzleCoroutine()
    {
        PuzzleFailed = false;
        for (int i = 0; i < MaxMove; i++)
        {
            yield return Step();
            if (PuzzleFailed)
            {
                break;
            }
        }
        SetSolved();
        if (!WasSolved)
        {
            yield return new WaitForSeconds(.5f);
            OnReset(false);
        }
        EndPuzzle();
    }
    IEnumerator Step()
    {
        foreach (RobotPawn robot in Robots)
        {
            robot.Step();
        }
        yield return new WaitForSeconds(1f);
        yield return new WaitWhile(() =>
        {
            foreach (RobotPawn robot in Robots)
            {
                if (robot.IsMoving())
                    return true;
            }
            return false;
        });
        foreach (RobotPawn robot in Robots)
        {
            if (robot.HasCrashed())
            {
                PuzzleFailed = true;
                break;
            }
        }
        
    }
    public bool IsPlaying()
    {
        return PlayCoroutine != null;
    }
    public void EndPuzzle()
    {
        if (IsPlaying())
        {
            StopCoroutine(PlayCoroutine);
        }
        PlayCoroutine = null;
    }

    #endregion
    #region MaxMove
    int MaxMove = 1;
    public void UpdateMoveLimit(int nLimit)
    {
        MaxMove = Mathf.Max(nLimit, MaxMove);
    }
    #endregion
    #region Solution
    public override bool CheckSolved()
    {
        if (base.CheckSolved())
            return true;

        if (!PuzzleFailed)
        {
            bool solved = true;
            foreach (ButtonPawn bp in Objectives)
            {
                if (!bp.IsPressed())
                {
                    solved = false;
                    break;
                }
            }
            return solved;
        }
        return false;
    }
    #endregion
}
