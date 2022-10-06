using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class CinematicsController : MonoBehaviour
{
    public bool PlayOnAwake = false;
    public bool MakePlayerInvisible = false;
    public PlayableDirector Director;

    public UnityEvent OnStart;
    public UnityEvent OnFinish;

    private void Start()
    {
        if (PlayOnAwake)
        {
            StartCinematic();
        }
    }
    void OnEnable()
    {
        if (Director == null)
            Director = GetComponent<PlayableDirector>();
        if (Director != null)
            Director.stopped += EndCinematic;
    }

    void OnDisable()
    {
        if (Director != null)
            Director.stopped -= EndCinematic;
    }
    public void StartCinematic()
    {
        if (Director!=null)
        {
            Director.Play();
        }
        if (PlayerCinematicController.main!=null)
        {
            PlayerCinematicController.main.SetCinematicMode(true, MakePlayerInvisible);
        }
        OnStart.Invoke();
    }

    void EndCinematic(PlayableDirector aDirector)
    {
            if (PlayerCinematicController.main != null)
        {
            PlayerCinematicController.main.SetCinematicMode(false, false);
            }
        OnFinish.Invoke();
    }
}
