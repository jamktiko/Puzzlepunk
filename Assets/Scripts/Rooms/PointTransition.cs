using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointTransition : MonoBehaviour
{
    public NavPoint navPoint;
    public bool instant;

    public void TriggerTransition()
    {
        PlayerTransitionController.main.TeleportToPoint(navPoint, instant);
    }
}
