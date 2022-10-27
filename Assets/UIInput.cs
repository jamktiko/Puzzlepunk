using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class UIInput : MonoBehaviour
{
    public InputActionReference action;
    public UnityEvent onPress;
    public UnityEvent onHold;
    public UnityEvent onRelease;

    Action<InputAction.CallbackContext> fPress;
    Action<InputAction.CallbackContext> fHold;
    Action<InputAction.CallbackContext> fRelease;
    private void OnEnable()
    {
        fPress = _ => { if (onPress != null) onPress.Invoke(); };
        fHold = _ => { if (onHold != null) onHold.Invoke(); };
        fRelease = _ => { if (onRelease != null) onRelease.Invoke(); };
        if (action != null) {
            action.action.started += fPress;
            action.action.started += fHold;
            action.action.canceled += fRelease;
        }
        fRelease(new InputAction.CallbackContext());
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
}
