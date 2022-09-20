using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotMenu : MonoBehaviour
{
    public RobotPuzzleController myPuzzle;
    public OrderUI orderMenu;
    public IconUI iconMenu;

    private void Awake()
    {
        if (orderMenu == null)
            orderMenu = GetComponentInChildren<OrderUI>();
        if (iconMenu == null)
            iconMenu = GetComponentInChildren<IconUI>();
    }
    public void InitPuzzle(RobotPuzzleController puzzle)
    {
        myPuzzle = puzzle;
        iconMenu.OnPuzzleStart();
    }
    public void OnReset(bool soft)
    {
        myPuzzle.OnReset(soft);
    }
    public bool IsPuzzleMode()
    {
        return gameObject.activeSelf && myPuzzle != null;
    }
    public void Close()
    {
        myPuzzle = null;
        UIController.main.CloseWindow();
    }
    public void OnSelectionChanged()
    {
        iconMenu.OnSelectionChanged();
    }
    public void PlaySolution()
    {
        if (PlayCoroutine!=null)
        {
            StopCoroutine(PlayCoroutine);
            OnReset(true);
        }
        PlayCoroutine = StartCoroutine(PuzzleCoroutine());
    }
    Coroutine PlayCoroutine;
    IEnumerator PuzzleCoroutine()
    {
        foreach (var robot in myPuzzle.Robots)
        {
            robot.Step();
        }
    }
    IEnumerator Step()
    {

    }
}
