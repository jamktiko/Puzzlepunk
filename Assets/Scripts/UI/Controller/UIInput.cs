using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class UIInput : MonoBehaviour
{
    public bool GamepadOnly = false;
    public InputActionReference action;
    public UnityEvent onPress;
    public UnityEvent onHold;
    public UnityEvent onRelease;

    Action<InputAction.CallbackContext> fPress;
    Action<InputAction.CallbackContext> fHold;
    Action<InputAction.CallbackContext> fRelease;
    private void Awake()
    {
        fPress = _ => {
            if (!GamepadOnly || IsControllerConnected())
                if (onPress != null) onPress.Invoke();
        };
        fHold = _ => {
            if (!GamepadOnly || IsControllerConnected())
                if (onHold != null) onHold.Invoke();
        };
        fRelease = _ => {
            if (!GamepadOnly || IsControllerConnected())
                if (onRelease != null) onRelease.Invoke();
        };
        onRelease.Invoke();
    }
    private void OnEnable()
    {
        if (action != null)
        {
            action.action.started += fPress;
            action.action.started += fHold;
            action.action.canceled += fRelease;
        }
    }
    private void OnDisable()
    {
        if (action != null)
        {
            action.action.started -= fPress;
            action.action.started -= fHold;
            action.action.canceled -= fRelease;
        }
    }
    bool IsControllerConnected()
    {
        foreach (PlayerInput control in PlayerInput.all)
        {
            switch (control.currentControlScheme)
            {
                case "Gamepad":
                    return true;
                default:
                    continue;

            }
        }
        return false;
    }
}
