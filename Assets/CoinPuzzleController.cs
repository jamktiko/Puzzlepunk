using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPuzzleController : MonoBehaviour
{
    CoinController[] coins;

    private void Awake()
    {
        coins = GetComponentsInChildren<CoinController>();
    }
    public void TrySwapCoin(CoinController coin)
    {

    }
}
