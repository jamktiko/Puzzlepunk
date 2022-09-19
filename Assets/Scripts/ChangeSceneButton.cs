using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneButton : MonoBehaviour
{
    public string GoToScene;
    public void ChangeScene()
    {
        SceneManager.LoadScene(GoToScene);
    }
}
