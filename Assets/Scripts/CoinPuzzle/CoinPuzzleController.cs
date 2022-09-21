using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class CoinPuzzleController : MonoBehaviour
{
    public bool IsSolved = false;
    CoinController[] coins;
    public UnityEvent onSolved;

    private void Awake()
    {
        coins = GetComponentsInChildren<CoinController>();
        foreach (CoinController c in coins)
            c.TieToPuzzle(this);
    }
    public void TrySwapCoin(CoinController othercoin)
    {
        foreach(CoinController coin in coins)
        {
            if (coin.IsEmpty() != othercoin.IsEmpty() && coin != othercoin)
            {
                float MinDist = Mathf.Max(coin.SwapRange, othercoin.SwapRange);
                    if ((coin.RectT.position - othercoin.RectT.position).sqrMagnitude < MinDist * MinDist){
                    othercoin.Swap(coin);
                    return;
                }
            }
        }
    }
    public void CheckSolved()
    {
        if (!IsSolved)
        {
            foreach (CoinController coin in coins)
            {
if (coin.IsImportant && coin.CoinNumber != coin.RequiresNumber)
                {
                    return;
                }
            }
        }
        Solve();
    }
    void Solve()
    {
        if (!IsSolved)
        {
            IsSolved = true;
            onSolved.Invoke();
            Debug.Log("PUZZLE SOLVED!");
        }
    }
}
