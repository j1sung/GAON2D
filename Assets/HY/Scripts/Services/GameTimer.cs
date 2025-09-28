using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimer
{
    public float TimeLeft { get; private set; }
    public bool IsRunning { get; private set; }

    public GameTimer(float durationSec)
    {
        TimeLeft = Mathf.Max(0, durationSec);
        IsRunning = false;
    }
    public void Start() => IsRunning = true;
    public void Stop() => IsRunning = false;
    public void Reset(float durationSec) { TimeLeft = durationSec; IsRunning = false; }

    public bool Tick()
    { 
        if (!IsRunning) return false;
        TimeLeft -= UnityEngine.Time.deltaTime;
        if (TimeLeft <= 0f) { TimeLeft = 0f; IsRunning = false; return true; }
        return false;
    }
}
