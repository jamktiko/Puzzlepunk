using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static RobotOrderButton;

public class RobotOrderButton : MonoBehaviour
{
    public Sprite[] States;
    public Sprite[] Arrows;
    public Image BackgroundImage;
    public Image ArrowImage;

    public enum ButtonState
    {
        used = 0,
        current = 1,
        remaining = 2,
        inactive = 3
    }

    public void ChangeOrder(int arrowSprite)
    {
        ArrowImage.sprite = Arrows[arrowSprite];
        ArrowImage.gameObject.SetActive(arrowSprite != 0);
    }
    public void ChangeState(ButtonState buttonState)
    {
        BackgroundImage.sprite = States[(int)buttonState];
        gameObject.SetActive(buttonState != ButtonState.inactive);
    }
    public void SetBroken(bool value)
    {
        BackgroundImage.color = value ? Color.red : Color.white;
    }
}
