using System.Collections;
using System.Collections.Generic;
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

    public bool PlayOnce = false;
    public bool MakePlayerInvisible = false;
    public PlayableDirector Director;

    public UnityEvent OnStart;
    public UnityEvent OnFinish;

    bool hasPlayed = false;

    private void Awake()
    {
        if (Director == null)
            Director = GetComponent<PlayableDirector>();
        if (Director != null)
            Director.stopped += EndCinematic;
    }

    private void Update()
    {
        bool skip = PlayerInputListener.control.ZoePlayer.Skip.ReadValue<float>()>0;
        if (currentMode == PlayMode.playing && skip)
        {
            SetPlayMode(PlayMode.fast);
        }
        if (currentMode == PlayMode.fast && !skip)
        {
            SetPlayMode(PlayMode.playing);
        }
    }
    public void StartCinematic()
    {
        if (hasPlayed && PlayOnce)
            return;
        active = this;
        if (Director!=null)
        {
            Director.Play();
            SetPlayMode(PlayMode.playing);
        if (PlayerCinematicController.main!=null)
        {
            PlayerCinematicController.main.SetCinematicMode(true, MakePlayerInvisible);
        }
        }
        OnStart.Invoke();
    }

    public void SetPlayerInvisible()
    {
        if (PlayerCinematicController.main != null)
        {
            PlayerCinematicController.main.SetCinematicMode(true, true);
        }
    }

    void EndCinematic(PlayableDirector aDirector)
    {
        hasPlayed = true;
        OnFinish.Invoke();
        SetPlayMode(PlayMode.stopped);
        if (PlayerCinematicController.main != null)
        {
            PlayerCinematicController.main.SetCinematicMode(false, false);
        }
        if (active == this)
            active = null;
    }
    PlayMode currentMode = PlayMode.stopped;
    public void SetPlayMode(PlayMode mode)
    {
        currentMode = mode;
        if (currentMode > PlayMode.stopped && Director != null && Director.playableGraph.IsValid())
        {
            Director.playableGraph.GetRootPlayable(0).SetSpeed((float)mode);
        }
    }
}
