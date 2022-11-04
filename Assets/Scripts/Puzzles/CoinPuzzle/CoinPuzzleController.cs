using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class CoinPuzzleController : PuzzleController
{

    CoinController[] coins;
    CoinController[] objectivecoins;
    CoinRotator[] pieces;

    private void Awake()
    {
        coins = GetComponentsInChildren<CoinController>();
        foreach (CoinController c in coins)
            c.TieToPuzzle(this);
        pieces = GetComponentsInChildren<CoinRotator>();
        foreach (CoinRotator r in pieces)
            r.TieToPuzzle(this);

        InitSolution();
    }
    void InitSolution()
    {
        List<CoinController> important = new List<CoinController>();
        important.AddRange(coins);
        important.RemoveAll((CoinController C) => { return !C.IsImportant; });
        objectivecoins = important.ToArray();
    }
    public void TrySwapCoin(CoinController othercoin)
    {
        foreach(CoinController coin in coins)
        {
            if (coin.IsEmpty() != othercoin.IsEmpty() && coin != othercoin)
            {
                float MinDist = Mathf.Max(coin.SwapRange, othercoin.SwapRange) * transform.localScale.y;
                    if ((coin.RectT.position - othercoin.RectT.position).sqrMagnitude < MinDist * MinDist){
                    othercoin.Swap(coin);
                    return;
                }
            }
        }
    }
    private void OnEnable()
    {
        OnEnterPuzzle();
    }
    private void OnDisable()
    {
        OnExitPuzzle();
    }
    public override bool CheckSolved()
    {
        if (base.CheckSolved())
            return true;
        for (int iA = 0; iA < objectivecoins.Length; iA++)
        {
            bool solved = true;
            for (int iB = 0; iB < objectivecoins.Length; iB++)
            {
                CoinController coin = objectivecoins[(iB + iA) % objectivecoins.Length];
                if (coin.CoinNumber != objectivecoins[iB].RequiresNumber)
                {
                    solved = false;
                    break;
                }
            }
            if (solved)
            {
                foreach (CoinRotator Rotator in pieces)
                {
                    if (!Rotator.IsInRequiredRotation(iA))
                        return false;
                }
                return true;
            }
        }
        
        return false;
    }
}
