using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public bool RequireGameEssentials = false;
    public string LoadingScene;
    public string GameEssentials;

    public static SceneTransitionManager main;
    private void Awake()
    {
        if (main == null)
        {
            main = this;
            GameObject.DontDestroyOnLoad(gameObject);
            if (RequireGameEssentials)
                SceneManager.LoadSceneAsync(GameEssentials, LoadSceneMode.Additive);
        }
        else
        {
            GameObject.DestroyImmediate(this);
        }
    }
    public void TransitionScene(string SceneName)
    {
        StartCoroutine(TransitionSceneCoroutine(SceneName));
    }
    public IEnumerator TransitionSceneCoroutine(string SceneName)
    {
        yield return SceneManager.LoadSceneAsync(LoadingScene);
        yield return new WaitForSecondsRealtime(.5f);
        yield return SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);
        yield return new WaitForSecondsRealtime(.5f);
        SceneManager.UnloadSceneAsync(LoadingScene);
    }
    public void TransitionGameScene(string SceneName)
    {
        StartCoroutine(TransitionGameSceneCoroutine(SceneName));
    }
    public IEnumerator TransitionGameSceneCoroutine(string SceneName)
    {
        yield return SceneManager.LoadSceneAsync(LoadingScene);
        yield return new WaitForSecondsRealtime(.5f);
        yield return SceneManager.LoadSceneAsync(GameEssentials, LoadSceneMode.Additive);
        yield return SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);
        yield return new WaitForSecondsRealtime(.5f);
        SceneManager.UnloadSceneAsync(LoadingScene);
    }
}
