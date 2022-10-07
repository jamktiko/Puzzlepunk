using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using static UIController;

public class CinematicsController : MonoBehaviour
{
    public static CinematicsController active;
    public enum PlayMode
    {
        stopped = -1,
        pause = 0,
        playing = 1,
        fast = 2
    }

    public bool PlayOnStart = false;
    public bool MakePlayerInvisible = false;
    public PlayableDirector Director;

    public UnityEvent OnStart;
    public UnityEvent OnFinish;

    private void Start()
    {
        if (PlayOnStart)
        {
            StartCinematic();
        }
        if (Director == null)
            Director = GetComponent<PlayableDirector>();
        if (Director != null)
            Director.stopped += EndCinematic;
    }
    void OnEnable()
    {

        if (PlayOnStart && currentMode != PlayMode.stopped)
        {
            StartCinematic();
        }
        active = this;
    }

    void OnDisable()
    {
        if (active == this)
        active = null;
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
        if (Director!=null)
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
        SetPlayMode(PlayMode.stopped);
        OnFinish.Invoke();
    }
    PlayMode currentMode = PlayMode.playing;
    public void SetPlayMode(PlayMode mode)
    {
        currentMode = mode;
        if (currentMode> PlayMode.stopped && Director != null && Director.playableGraph.IsValid())
        {
            Director.playableGraph.GetRootPlayable(0).SetSpeed((float)mode);
        }
    }
}
