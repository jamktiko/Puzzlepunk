using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCinematicController : MonoBehaviour
{
    public static PlayerCinematicController main;
    private void Awake()
    {
        main = this;
    }
    bool CinematicMode = false;
    public void EnableInvisibleMode()
    {
        SetCinematicMode(true,true);
    }
    public void EnableCinematicMode()
    {
        SetCinematicMode(true, false);
    }
    public void DisableCinematicMode()
    {
        SetCinematicMode(false, false);
    }
    public void SetCinematicMode(bool toValue, bool invisible)
    {
        PlayerMovement.main.movement.Stop();
        CinematicMode = toValue;
        gameObject.SetActive(!invisible);

        PlayerAnimations.main.SetWalking(false);
        PlayerAnimations.main.SetCurious(false);

        if (UIController.main != null)
            UIController.main.ToggleHelpOverlay(!toValue);
    }
    public bool IsInCinematicMode()
    {
        return CinematicMode;
    }
    public bool IsInInvisibeMode()
    {
        return gameObject.activeSelf;
    }
}
