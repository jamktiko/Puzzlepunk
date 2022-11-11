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
                StartCoroutine(LoadSceneAsync(GameEssentials, LoadSceneMode.Additive));
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
        yield return LoadSceneAsync(LoadingScene);
        yield return new WaitForSecondsRealtime(.5f);
        yield return LoadSceneAsync(SceneName, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(LoadingScene);
    }
    public void TransitionGameScene(string SceneName)
    {
        StartCoroutine(TransitionGameSceneCoroutine(SceneName));
    }
    public IEnumerator TransitionGameSceneCoroutine(string SceneName)
    {
        yield return LoadSceneAsync(LoadingScene);
        yield return new WaitForSecondsRealtime(.5f);
        yield return LoadSceneAsync(GameEssentials, LoadSceneMode.Additive);
        yield return LoadSceneAsync(SceneName, LoadSceneMode.Additive);
        if (LoadingScreenManager.main != null)
            LoadingScreenManager.main.SetLoaded(true);
    }
    IEnumerator LoadSceneAsync(string sceneName)
    {
        yield return LoadSceneAsync(sceneName, LoadSceneMode.Single);
    }
        IEnumerator LoadSceneAsync(string sceneName, LoadSceneMode sceneMode)
    {
        for (int iS = 0; iS < SceneManager.sceneCount; iS++)
            if (SceneManager.GetSceneAt(iS).name == sceneName)
                yield return null;
        yield return SceneManager.LoadSceneAsync(sceneName, sceneMode);
    }
}
