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
        SceneManager.sceneLoaded += OnSceneChange;
        gameObject.SetActive(false);
    }
    void OnSceneChange(Scene scene, LoadSceneMode mode)
    {
        variables.UpdateReactors();
    }
    public void StartGame()
    {
        gameObject.SetActive(true);
        LevelController.main.BeginGame();
    }

}
