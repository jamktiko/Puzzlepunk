using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneButton : MonoBehaviour
{
    public string MainScene;
    public string[] AdditiveScenes;
    public void ChangeScene()
    {
        SceneManager.LoadScene(MainScene);
        foreach (string SceneName in AdditiveScenes)
        {
            SceneManager.LoadScene(SceneName, LoadSceneMode.Additive);
        }
    }
}
