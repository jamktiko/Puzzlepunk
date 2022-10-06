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
        CinematicMode = toValue;
        gameObject.SetActive(!invisible);
        Debug.Log("Cinematic mode " + toValue);
    }
    public bool IsInCinematicMode()
    {
        return CinematicMode;
    }
}
