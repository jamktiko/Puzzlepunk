using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleNumberController : PuzzlePiece
{
    int myDigit = 0;
    public Button buttonUp;
    public Button buttonDown;
    public TextMeshProUGUI text;
    public override void OnReset(bool hard)
    {
        if (hard)
        {
            myDigit = 0;
        }
        UpdateText();
    }
    public int GetDigit()
    {
        return myDigit;
    }
    public void ChangeDigitUp()
    {
        myDigit++;
        if (myDigit > 9)
            myDigit = 0;
        UpdateText();
        puzzleParent.CheckSolved();
    }
    public void ChangeDigitDown()
    {
        myDigit--;
        if (myDigit < 0)
            myDigit = 9;
        UpdateText();
        puzzleParent.CheckSolved();
    }
    void UpdateText()
    {
        text.text = myDigit.ToString();
    }
}
