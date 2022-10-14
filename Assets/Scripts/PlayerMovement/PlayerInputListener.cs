using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputListener : MonoBehaviour
{
    public static ZoeControlls control;
    void Awake()
    {
        control = new ZoeControlls ();

        control.ZoePlayer.Enable();
        control.ZoePlayer.Movement.Enable();
        control.ZoePlayer.Skip.Enable();
        control.ZoePlayer.Submit.Enable();
        control.ZoePlayer.Interact.Enable();
    }
}
