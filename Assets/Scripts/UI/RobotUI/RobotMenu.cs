using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotMenu : MonoBehaviour
{
    public RobotPuzzleController myPuzzle;
     OrderUI orderMenu;
     IconUI iconMenu;
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
        myPuzzle.EndPuzzle();
        myPuzzle.OnReset(hard);
        ResetOrderUI();
    }
    public void OnBackSpace()
    {
        if (myPuzzle.IsPlaying()) return;
        myPuzzle.GetSelectedRobot().ClearLastOrder();
        ResetOrderUI();
    }
    public bool IsPuzzleMode()
    {
        return gameObject.activeSelf && myPuzzle != null;
    }
    public void Close()
    {
        if (myPuzzle.ClosePuzzle())
        {
            myPuzzle = null;
            UIController.main.CloseWindow(true);
        }
    }
    public void OnSelectionChanged()
    {
        iconMenu.OnSelectionChanged();
        ResetOrderUI();
    }
    public void ResetOrderUI()
    {
        orderMenu.UpdateOrders();
        iconMenu.UpdateOrders();
    }
    public void PlaySolution()
    {
        myPuzzle.PlaySolution();
    }
}
