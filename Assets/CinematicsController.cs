using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using static UIController;

public class CinematicsController : MonoBehaviour
{
    public enum PlayMode
    {
        pause = 0,
        playing = 1,
        fast = 2
    }


    public bool PlayOnAwake = false;
    public bool MakePlayerInvisible = false;
    public PlayableDirector Director;

    public UnityEvent OnStart;
    public UnityEvent OnFinish;

    private void Awake()
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

        if (PlayOnAwake)
        {
            StartCinematic();
        }
    }

    void OnDisable()
    {
        if (Director != null)
            Director.stopped -= EndCinematic;
    }
    private void Update()
    {
        if (currentMode == PlayMode.playing && Input.GetButton("Skip"))
        {
            SetPlayMode(PlayMode.fast);
        }
        if (currentMode == PlayMode.fast && !Input.GetButton("Skip"))
        {
            SetPlayMode(PlayMode.playing);
        }
    }
    public void StartCinematic()
    {
        if (Director!=null && Director.state != PlayState.Playing)
        {
            Director.Play();
            SetPlayMode(PlayMode.playing);
        if (PlayerCinematicController.main!=null)
        {
            PlayerCinematicController.main.SetCinematicMode(true, MakePlayerInvisible);
        }
        OnStart.Invoke();
        }
    }

    void EndCinematic(PlayableDirector aDirector)
    {
        if (PlayerCinematicController.main != null)
        {
            PlayerCinematicController.main.SetCinematicMode(false, false);
        }
        OnFinish.Invoke();
    }
    PlayMode currentMode = PlayMode.playing;
    public void SetPlayMode(PlayMode mode)
    {
        currentMode = mode;
        if (Director != null && Director.playableGraph.IsValid())
        {
            Director.playableGraph.GetRootPlayable(0).SetSpeed((int)mode);
        }
    }
}
