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
    private void Awake()
    {
        fPress = _ => {
            if (onPress != null) onPress.Invoke();
        };
        fHold = _ => {
            if (onHold != null) onHold.Invoke();
        };
        fRelease = _ => {
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
}
