using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController main;
    public VariableManager variables;
    private void Awake()
    {
        main = this;
        variables = new VariableManager();
    }
}
