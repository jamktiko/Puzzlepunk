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
    public Image RobotIcon;
    public Button RobotButton;
    public TextMeshProUGUI buttonLabel;

    public void ChangeIcon(Sprite icon)
    {
        RobotIcon.sprite = icon;
    }
    /*public void ChangeIcon(Color col)
    {
        RobotIcon.color = col;
    }*/
    public void SetSelected(bool state)
    {
        RobotBorders.sprite = state ? SpriteSelected : SpriteDeselected;
    }
    public void ChangeLabel(int current, int remaining)
    {
        buttonLabel.text = current + "/" + remaining;
    }
}
