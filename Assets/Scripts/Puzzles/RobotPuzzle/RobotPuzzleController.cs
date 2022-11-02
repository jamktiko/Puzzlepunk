using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RobotPuzzleController : PuzzleController
{
    public PuzzlePawn[] Pieces;
    public RobotPawn[] Robots;
    public ButtonPawn[] Objectives;
    public GridNav mGrid;
    public CameraBounds cambounds;
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
    public void ChangeSelection(int sel)
    {
        Selection = sel;
        UIController.main.robotController.OnSelectionChanged();
    }
    public void OnReset(bool hard)
    {
        foreach (PuzzlePawn rob in Pieces)
        {
            rob.OnReset(hard);
        }
    }
    public int Selection = 0;
    public RobotPawn GetSelectedRobot()
    {
        return Robots[Selection];
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
                yield return new WaitForSeconds(.5f);
                OnReset(false);
                break;
            }
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
        if (!PuzzleFailed)
        {
            SetSolved();
        }
    }
    public void EndPuzzle()
    {
        if (PlayCoroutine != null)
        {
            StopCoroutine(PlayCoroutine);
        }
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
    #endregion
}
