using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinRotator : MonoBehaviour
{
    public float RotationAngle = 45f;
    public float RotationTime = 1f;    
    CoinController[] coins;

    private void Awake()
    {
        coins = GetComponentsInChildren<CoinController>();
    }
    void OnRotate()
    {
        foreach (CoinController c in coins)
        {
            c.transform.rotation = Quaternion.identity;
        }
    }
    public void Rotate(bool Clockwise)
    {
        if (RotationCoroutine != null)
            StopCoroutine(RotationCoroutine);
        RotationCoroutine = StartCoroutine(RotateDirection(RotationAngle * (Clockwise ? 1 : -1), RotationTime));
    }

    float DesiredAngle = 0;
    Coroutine RotationCoroutine;
    IEnumerator RotateDirection(float angle, float time)
    {
        DesiredAngle += angle;
        float rTime = time;
        while (rTime > 0)
        {
            rTime -= Time.deltaTime;
            transform.rotation *= Quaternion.Euler(0, 0, angle / time * Time.deltaTime);
            OnRotate();
            yield return new WaitForEndOfFrame();
        }
        transform.rotation = Quaternion.Euler(0, 0, DesiredAngle);
    }
}
