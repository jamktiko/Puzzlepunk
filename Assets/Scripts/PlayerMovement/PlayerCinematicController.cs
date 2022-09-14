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
    public void EnableCinematicMode()
    {
        SetCinematicMode(true);
    }
    public void DisableCinematicMode()
    {
        SetCinematicMode(false);
    }
    public void SetCinematicMode(bool toValue)
    {
        CinematicMode= toValue;
        Debug.Log("Cinematic mode " + toValue);
    }
    public bool IsInCinematicMode()
    {
        return CinematicMode;
    }
}
