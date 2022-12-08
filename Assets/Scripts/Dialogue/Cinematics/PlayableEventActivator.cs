using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

// A behaviour that is attached to a playable
public class PlayableEventActivator : PlayableBehaviour
{

    public UnityEvent OnStart;
    // Called when the owning graph starts playing
    public override void OnGraphStart(Playable playable)
    {
        OnStart.Invoke();
    }

    public UnityEvent OnStop;
    // Called when the owning graph stops playing
    public override void OnGraphStop(Playable playable)
    {
        OnStop.Invoke();
    }

    // Called when the state of the playable is set to Play
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        
    }

    // Called when the state of the playable is set to Paused
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        
    }

    public UnityEvent OnFrame;
    // Called each frame while the state is set to Play
    public override void PrepareFrame(Playable playable, FrameData info)
    {
        OnFrame.Invoke();
    }
}
