using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconUI : MonoBehaviour
{
    public Image[] RobotBorders;
    public Image[] RobotIcons;

    private void Awake()
    {
        RobotBorders = new Image[transform.childCount];
        RobotIcons = new Image[transform.childCount];

        int iC = 0;
        foreach (Transform child in transform)
        {
            RobotBorders[iC] = child.GetComponent<Image>();
            RobotIcons[iC] = child.GetChild(0).GetComponent<Image>();
            iC++;
        }
    }
    public void OnPuzzleStart()
    {
        var puzzle = UIController.main.robotController.myPuzzle;
        for (int iI = 0; iI< RobotIcons.Length; iI++)
        {
            bool RobotExists = iI < puzzle.Robots.Length;
            if (RobotExists)
            {
                RobotIcons[iI].sprite = puzzle.Robots[iI].GetSprite();
                    }
            RobotBorders[iI].gameObject.SetActive(RobotExists);
        }
    }
    public void OnSelectionChanged()
    {
        int sel = UIController.main.robotController.myPuzzle.Selection;
        for (int iI = 0; iI < RobotIcons.Length; iI++)
        {
            RobotIcons[iI].color = ((iI == sel) ? Color.yellow : Color.gray);
        }
    }
}
