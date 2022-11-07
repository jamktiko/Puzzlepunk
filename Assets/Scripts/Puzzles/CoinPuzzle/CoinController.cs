using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinController : PuzzlePiece
{
    public RectTransform RectT;
    public Image cImage;
    public TextMeshProUGUI cText;
    public float SwapRange = 100;
    private void Awake()
    {
        if (cImage==null)
            cImage = transform.GetChild(0).GetComponent<Image>();
        if (cText == null)
            cText = GetComponentInChildren<TextMeshProUGUI>();
        if (RectT == null)
            RectT = GetComponent<RectTransform>();
        InitialNumber = CoinNumber;
    }
    public int CoinNumber = 0;
    int InitialNumber = 0;
    private void Start()
    {
        UpdateCoinDisplay();
    }
    public override void OnReset(bool hard)
    {
        if (hard)
        {
            CoinNumber = InitialNumber;
            UpdateCoinDisplay();
        }
    }
    void UpdateCoinDisplay()
    {
        cImage.enabled = IsEmpty();
        cText.enabled = IsEmpty();
        cText.text = CoinNumber.ToString();
    }
    public bool IsEmpty()
    {
        return CoinNumber >= 0;
    }
    public void OnClicked()
    {
        if ((CoinPuzzleController)puzzleParent != null)
           ( (CoinPuzzleController)puzzleParent).TrySwapCoin(this);
    }
    public void Swap(CoinController otherC)
    {
        int coinA = CoinNumber;
        int coinB = otherC.CoinNumber;

        CoinNumber = coinB;
        otherC.CoinNumber = coinA;

        UpdateCoinDisplay();
        otherC.UpdateCoinDisplay();

        if (puzzleParent != null)
            puzzleParent.CheckSolved();
    }
    public bool IsImportant = false;
    public int RequiresNumber = 0;
}
