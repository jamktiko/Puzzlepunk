using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconUI : MonoBehaviour
{
    public RobotIconButton[] RobotButtons;

    private void Awake()
    {
        RobotButtons = GetComponentsInChildren<RobotIconButton>();

        int iC = 0;
        foreach (RobotIconButton child in RobotButtons)
        {
            int iSelection = iC;
            child.RobotButton.onClick.AddListener(() =>
            {
                UIController.main.robotController.myPuzzle.ChangeSelection(iSelection);
                OnSelectionChanged();
            });

            iC++;
        }
    }
    public void CycleSelection(bool back)
    {
        UIController.main.robotController.myPuzzle.CycleSelection(back ? -1 : 1);
        OnSelectionChanged();
    }
    public void OnPuzzleStart()
    {
        RobotPuzzleController puzzle = UIController.main.robotController.myPuzzle;
        for (int iI = 0; iI< RobotButtons.Length; iI++)
        {
            bool RobotExists = iI < puzzle.RobotCommands.Length && puzzle.RobotCommands[iI]!=null;
            if (RobotExists)
            {
                RobotButtons[iI].ChangeIcon(puzzle.RobotCommands[iI].Icon);
            }
            RobotButtons[iI].gameObject.SetActive(RobotExists);
        }
    }
    public void OnSelectionChanged()
    {
        int sel = UIController.main.robotController.myPuzzle.Selection;
        for (int iI = 0; iI < RobotButtons.Length; iI++)
        {
            RobotButtons[iI].SetSelected(iI == sel);
            //RobotButtons[iI].color = ((iI == sel) ? Color.yellow : Color.gray);
        }
    }
    public void UpdateOrders()
    {
        var puzzle = UIController.main.robotController.myPuzzle;
        for (int iI = 0; iI < RobotButtons.Length; iI++)
        {
            if (iI < puzzle.RobotCommands.Length && puzzle.RobotCommands[iI]!=null)
            {
                RobotButtons[iI].ChangeLabel(puzzle.RobotCommands[iI].iOrder, puzzle.RobotCommands[iI].orders.Length);
            }
        }
    }
}
