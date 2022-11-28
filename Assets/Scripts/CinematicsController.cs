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
        fast = 4
    }

    public bool PlayOnce = false;
    public bool PlayOnStart = false;
    public bool MakePlayerInvisible = false;
    public bool CanOverlap = false;
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

    void Start()
    {
        if (PlayOnStart)
        {
            StartCinematic();
        }
    }
    void OnEnable()
    {
        if (PlayOnStart)
        {
            StartCinematic();
        }
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
        if (!CanOverlap && PlayerCinematicController.main != null && PlayerCinematicController.main.IsInCinematicMode())
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
        else
            { Debug.LogError("PlayerCinematicController.main does not exist!"); }
        }
        OnStart.Invoke();
    }

    public void ChangePlayerVisibility(bool value)
    {
        if (PlayerCinematicController.main != null)
        {
            PlayerCinematicController.main.SetCinematicMode(true, value);
        }
    }

    void EndCinematic(PlayableDirector aDirector)
    {
        hasPlayed = true;
        if (PlayerCinematicController.main != null)
        {
            PlayerCinematicController.main.SetCinematicMode(false, false);
        }
        OnFinish.Invoke();
        SetPlayMode(PlayMode.stopped);
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
