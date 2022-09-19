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
    }
    public void HandleReset()
    {
        myPuzzle.Reset();
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
}
