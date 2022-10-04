using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointTransition : MonoBehaviour
{
    public NavPoint navPoint;
    public bool instant;

    public void MovePlayerToPoint()
    {
        PlayerTransitionController.main.TeleportToPoint(navPoint, instant);
    }
}
