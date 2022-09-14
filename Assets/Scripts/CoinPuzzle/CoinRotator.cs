using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinRotator : MonoBehaviour
{
    CoinController[] coins;

    private void Awake()
    {
        coins = GetComponentsInChildren<CoinController>();
    }
    private void Update()
    {
        transform.rotation *= Quaternion.Euler(0, 0, .3f);
        OnRotate();
    }
    void OnRotate()
    {
        foreach (CoinController c in coins)
        {
            c.transform.rotation = Quaternion.identity;
        }
    }
}
