using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerUIDisplay : MonoBehaviour
{
    public GameObject[] KeyboardTooltip;
    public GameObject[] ControllerTooltip;

    private void OnEnable()
    {
        bool contCon = IsControllerConnected();

        if (KeyboardTooltip != null)
            foreach (GameObject key in KeyboardTooltip)
                key.SetActive(!contCon);

        if (ControllerTooltip != null)
            foreach (GameObject cont in ControllerTooltip)
                cont.SetActive(contCon);
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
