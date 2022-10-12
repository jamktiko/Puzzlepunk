using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinRotator : MonoBehaviour
{
    public float RotationAngle = 45f;
    public float RotationTime = 1f;    
    CoinController[] coins;

    

    public CoinPuzzleController puzzleParent;
    public void TieToPuzzle(CoinPuzzleController parent)
    {
        puzzleParent = parent;
    }

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
        if (RotationCoroutine == null)
        RotationCoroutine = StartCoroutine(RotateDirection(RotationAngle * (Clockwise ? 1 : -1), RotationTime));
    }

    float DesiredAngle = 0;
    Coroutine RotationCoroutine;
    IEnumerator RotateDirection(float angle, float time)
    {
        DesiredAngle += angle;
        float RotationTime = Mathf.DeltaAngle(transform.rotation.eulerAngles.z, DesiredAngle) / angle;

        float rTime = time * RotationTime;
        while (rTime > 0)
        {
            rTime -= Time.deltaTime;
            transform.rotation *= Quaternion.Euler(0, 0, angle / time * Time.deltaTime);
            OnRotate();
            yield return new WaitForEndOfFrame();
        }
        transform.rotation = Quaternion.Euler(0, 0, DesiredAngle);
        OnRotate();
        puzzleParent.SetSolved();
        RotationCoroutine = null;
    }
    public bool IsInRequiredRotation(int iR)
    {
        float mRot = transform.rotation.eulerAngles.z;
        float coinDiff = 360f / coins.Length;

        return (Mathf.Round(mRot - coinDiff * iR) %360 == 0);
    }
}
