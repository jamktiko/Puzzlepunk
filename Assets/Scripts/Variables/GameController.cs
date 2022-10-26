using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController main;
    public VariableManager variables;
    private void Awake()
    {
        main = this;
        variables = new VariableManager();
    }
    private void Start()
    {

        if (LoadingScreenManager.main == null)
            StartGame();
        else
            gameObject.SetActive(false);
    }
    public void StartGame()
    {
        gameObject.SetActive(true);
        LevelController.main.BeginGame();
    }

}
