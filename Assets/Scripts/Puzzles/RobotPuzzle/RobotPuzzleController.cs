using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RobotPuzzleController : PuzzleController
{
    public float StepTime = .5f;
    public CameraBounds cambounds;
    public GridNav mGrid;

    public ButtonPawn[] Objectives;

    public RobotPawn[] Robots;
    public RobotPawn.Memory[] RobotCommands;

    protected override void InitSolution()
    {
        mGrid = GetComponentInChildren<GridNav>();

        Objectives = GetComponentsInChildren<ButtonPawn>();
        Robots = GetComponentsInChildren<RobotPawn>();//Todo nullchecks

        int maxCommand = 0;
        foreach (RobotPawn rob in Robots)
        {
            maxCommand = Mathf.Max(rob.CommandID, maxCommand);
        }
        RobotCommands = new RobotPawn.Memory[maxCommand+1];

        base.InitSolution();
    }

    protected override void OnEnterPuzzle()
    {
        base.OnEnterPuzzle();
        UIController.main.OpenWindow(UIController.UIWindow.robot);
        UIController.main.robotController.InitPuzzle(this);
        ChangeSelection(0);
        if (cambounds!=null)
        CameraController.main.SetBounds(cambounds);

    }
    protected override void OnExitPuzzle()
    {
        base.OnExitPuzzle();
        EndPuzzle();
        if (cambounds != null)
            CameraController.main.SetBounds(PlayerTransitionController.main.CurrentRoom.bounds);
    }
    public override bool ClosePuzzle()
    {
        OnExitPuzzle();
        return true;
    }
    #region Selection
    public int Selection = 0;
    public void ChangeSelection(int sel)
    {
        Selection = sel;
        while (RobotCommands[Selection] == null)
            Selection++;
        UIController.main.robotController.OnSelectionChanged();
    }
    public RobotPawn.Memory GetSelectedRobot()
    {
        return RobotCommands[Selection];
    }
    #endregion
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
        CheckSolved();
        if (!solved)
        {
            yield return new WaitForSeconds(StepTime);
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
    public override bool WasSolved()
    {
        if (base.WasSolved())
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
