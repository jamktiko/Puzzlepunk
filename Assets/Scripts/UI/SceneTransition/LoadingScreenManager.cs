using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreenManager : MonoBehaviour
{
    public GameObject LoadingScreen;
    public GameObject LoadingCompleteScreen;

    public static LoadingScreenManager main;
    private void Awake()
    {
        main = this;
        SetLoaded(false);
    }
    public void SetLoaded(bool value)
    {
        if (LoadingScreen!=null)
        LoadingScreen.gameObject.SetActive(!value);
        if (LoadingCompleteScreen != null)
            LoadingCompleteScreen.gameObject.SetActive(value);
    }
    public void OnLoadingDisbanded()
    {
        StartCoroutine(FadeToGameCoroutine());
    }
    IEnumerator FadeToGameCoroutine()
    {
        if (UIController.main !=null)
        {
            yield return UIController.main.TransitionScreen.AwaitTransitionIn(.5f);        
            UIController.main.TransitionScreen.TransitionOut(.5f);
        }
        GameController.main.StartGame();
        foreach (var gob in  SceneManager.GetSceneByName(SceneTransitionManager.main.LoadingScene).GetRootGameObjects())
        {
            gob.SetActive(false);
        }
        SceneManager.UnloadSceneAsync(SceneTransitionManager.main.LoadingScene);
        gameObject.SetActive(false);
    }
}
