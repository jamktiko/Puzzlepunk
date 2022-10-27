using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputListener : MonoBehaviour
{
    public static ZoeControlls control;
    void Awake()
    {
        control = new ZoeControlls ();
    }
    private void OnEnable()
    {
        control.ZoePlayer.Enable();
        control.ZoePlayer.Movement.Enable();
        control.ZoePlayer.Skip.Enable();
        control.ZoePlayer.Submit.Enable();
        control.ZoePlayer.Interact.Enable();
        control.ZoePlayer.Menu.Enable();
        control.ZoePlayer.ShowHelp.Enable();
    }
    private void OnDisable()
    {
        control.ZoePlayer.Disable();
        control.ZoePlayer.Movement.Disable();
        control.ZoePlayer.Skip.Disable();
        control.ZoePlayer.Submit.Disable();
        control.ZoePlayer.Interact.Disable();
        control.ZoePlayer.Menu.Disable();
        control.ZoePlayer.ShowHelp.Disable();
    }
}
