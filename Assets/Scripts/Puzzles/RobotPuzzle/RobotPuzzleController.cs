using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RobotPuzzleController : PuzzleController
{
    public RobotNPC[] Robots;
    public GridNav mGrid;
    public CameraBounds cambounds;
    private void Start()
    {
        Init();
    }
    private void Init()
    {
        mGrid = GetComponentInChildren<GridNav>();

        Robots = GetComponentsInChildren<RobotNPC>();//Todo nullchecks
        foreach (RobotNPC rob in Robots)
        {
            rob.InitPuzzle( this);
        }
        OnReset(false);
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
        foreach (RobotNPC rob in Robots)
        {
            rob.OnReset(hard);
        }
    }
    public int Selection = 0;
    public RobotNPC GetSelectedRobot()
    {
        return Robots[Selection];
    }
    #region Play
    bool PuzzleOver = false;
    public void PlaySolution()
    {
        EndPuzzle();
        PlayCoroutine = StartCoroutine(PuzzleCoroutine());
    }
    Coroutine PlayCoroutine;
    IEnumerator PuzzleCoroutine()
    {
        PuzzleOver = false;
        for (int i = 0; i < 10; i++)
        {
            yield return Step();
            if (PuzzleOver)
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
        foreach (RobotNPC robot in Robots)
        {
            robot.Step();
        }
        yield return new WaitForSeconds(1f);
        yield return new WaitWhile(() =>
        {
            foreach (RobotNPC robot in Robots)
            {
                if (robot.IsMoving())
                    return true;
            }
            return false;
        });
        foreach (RobotNPC robot in Robots)
        {
            if (robot.HasCrashed())
            {
                PuzzleOver = true;
                break;
            }
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
}
