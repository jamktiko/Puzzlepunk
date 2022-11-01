using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotMenu : MonoBehaviour
{
    public RobotPuzzleController myPuzzle;
    public OrderUI orderMenu;
    public IconUI iconMenu;
    public RobotInputListener input;

    private void Awake()
    {
        if (orderMenu == null)
            orderMenu = GetComponentInChildren<OrderUI>();
        if (iconMenu == null)
            iconMenu = GetComponentInChildren<IconUI>();
        if (input == null)
            input = GetComponentInChildren<RobotInputListener>();
    }
    public void InitPuzzle(RobotPuzzleController puzzle)
    {
        myPuzzle = puzzle;
        iconMenu.OnPuzzleStart();
    }
    public void OnReset(bool hard)
    {
        EndPuzzle();
        myPuzzle.OnReset(hard);
    }
    public bool IsPuzzleMode()
    {
        return gameObject.activeSelf && myPuzzle != null;
    }
    public void Close()
    {
        EndPuzzle();
        myPuzzle.OnExitPuzzle();
        myPuzzle = null;
        UIController.main.CloseWindow();
    }
    public void OnSelectionChanged()
    {
        iconMenu.OnSelectionChanged();
        orderMenu.UpdateOrders();
    }
    public void PlaySolution()
    {
        EndPuzzle();
        PlayCoroutine = StartCoroutine(PuzzleCoroutine());
    }
    Coroutine PlayCoroutine;
    IEnumerator PuzzleCoroutine()
    {
        for (int i = 0; i < 10; i++)
        {
            yield return Step();
        }
        EndPuzzle();
    }
    IEnumerator Step()
    {
        foreach (RobotNPC robot in myPuzzle.Robots)
        {
            robot.Step();
        }
        yield return new WaitForSeconds(1f);
        yield return new WaitWhile(() =>
        {
            foreach (RobotNPC robot in myPuzzle.Robots)
            {
                if (robot.IsMoving())
                    return true;
            }
            return false;
        } );
    }
    private void EndPuzzle()
    {
        if (PlayCoroutine != null)
        {
            StopCoroutine(PlayCoroutine);
        }
        OnReset(false);
    }
}
