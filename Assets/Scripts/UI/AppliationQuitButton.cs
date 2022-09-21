using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppliationQuitButton : MonoBehaviour
{
    public void OnPressed()
    {
#if UNITY_EDITOR
        Debug.Log("Application QUIT");
#else
        Application.Quit();   
#endif
    }
}
