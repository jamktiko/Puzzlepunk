using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CursorSelector : MonoBehaviour
{
    [Header("Config")]
    public bool ControllerOnly = true;
    public bool SelectOnEnable = false;

    [Header("Components")]
    public Selectable myButton;
    public InputActionReference SelectButton;

    private void Awake()
    {
     if (myButton == null)
        {
            myButton = GetComponent<Selectable>();
        }
     if (SelectButton!=null)
            SelectButton.action.started += _ => {
            CheckSelection();
        };
        if (SelectOnEnable)
            CheckSelection();
    }
    private void OnEnable()
    {
        if (SelectOnEnable)
        CheckSelection();
    }
    void CheckSelection()
    {
        if (ControllerOnly && !IsControllerConnected())
            return;
        if (EventSystem.current.currentSelectedGameObject == null || !EventSystem.current.currentSelectedGameObject.activeSelf || !EventSystem.current.currentSelectedGameObject.activeInHierarchy)
        {
            if (myButton != null)
            {
                myButton.Select();
                EventSystem.current.SetSelectedGameObject(myButton.gameObject);
            }
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
