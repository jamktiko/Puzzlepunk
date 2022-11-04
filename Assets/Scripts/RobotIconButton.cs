using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RobotIconButton : MonoBehaviour
{
    public Sprite SpriteSelected;
    public Sprite SpriteDeselected;

    public Image RobotBorders;
    public Image RobotIcons;
    public Button RobotButton;
    public TextMeshProUGUI buttonLabel;

    public void ChangeIcon(Sprite Icon)
    {
        RobotIcons.sprite = Icon;
    }
    public void SetSelected(bool state)
    {
        RobotBorders.sprite = state ? SpriteSelected : SpriteDeselected;
    }
    public void ChangeLabel(int current, int remaining)
    {
        buttonLabel.text = current + "/" + remaining;
    }
}
