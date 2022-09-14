using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinController : MonoBehaviour
{
    public Image cImage;
    public TextMeshProUGUI cText;
    private void Awake()
    {
        if (cImage==null)
        cImage = GetComponentInChildren<Image>();
        if (cText == null)
            cText = GetComponentInChildren<TextMeshProUGUI>();
    }
    public int CoinNumber = 0;
    private void Start()
    {
        UpdateCoinDisplay();
    }
    void UpdateCoinDisplay()
    {
        cImage.enabled = CoinNumber >= 0;
        cText.enabled = CoinNumber >= 0;
        cText.text = CoinNumber.ToString();
    }
}
