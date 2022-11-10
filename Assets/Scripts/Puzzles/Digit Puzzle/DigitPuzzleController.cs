using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigitPuzzleController : PuzzleController
{
    public int Solution = 0;
    PuzzleNumberController[] numbers;

    protected override void InitSolution()
    {
        numbers = GetComponentsInChildren<PuzzleNumberController>();
        base.InitSolution();
    }
    private void OnEnable()
    {
        OnEnterPuzzle();
    }
    private void OnDisable()
    {
        OnExitPuzzle();
    }
    public override bool WasSolved()
    {
        if (base.WasSolved())
            return true;
        int checksolution = 0;
        for (int iDigit = 0; iDigit < numbers.Length; iDigit++)
        {
            checksolution += numbers[iDigit].GetDigit() * (int)Mathf.Pow(10, numbers.Length - iDigit - 1);
        }
        return checksolution == Solution;
    }
}
