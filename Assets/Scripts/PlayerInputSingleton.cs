using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputSingleton : MonoBehaviour
{
    public static PlayerInputSingleton main;
    private void Awake()
    {
        if (main == null)
        {
            main = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
