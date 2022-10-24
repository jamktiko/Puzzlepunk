using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionButton : MonoBehaviour
{
    public void TransitionScene(string SceneName)
    {
       if (SceneTransitionManager.main!=null)
        {
            SceneTransitionManager.main.TransitionScene(SceneName);
        }
    }
    public void TransitionGameScene(string SceneName)
    {
        if (SceneTransitionManager.main != null)
        {
            SceneTransitionManager.main.TransitionGameScene(SceneName);
        }
    }
}
