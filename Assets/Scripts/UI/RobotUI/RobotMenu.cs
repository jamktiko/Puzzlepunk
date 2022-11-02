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
        myPuzzle.EndPuzzle();
        myPuzzle.OnReset(hard);
    }
    public void OnBackSpace()
    {
        if (myPuzzle.IsPlaying()) return;
        myPuzzle.GetSelectedRobot().ClearLastOrder();
        iconMenu.UpdateOrders();
        orderMenu.UpdateOrders();
    }
    public bool IsPuzzleMode()
    {
        return gameObject.activeSelf && myPuzzle != null;
    }
    public void Close()
    {
        myPuzzle.OnExitPuzzle();
        myPuzzle = null;
        UIController.main.CloseWindow();
    }
    public void OnSelectionChanged()
    {
        iconMenu.OnSelectionChanged();
        orderMenu.UpdateOrders();
        iconMenu.UpdateOrders();
    }
    public void PlaySolution()
    {
        myPuzzle.PlaySolution();
    }
}
