using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EventTimer
{
    private float currentTime = 0f;
    private float progress = 0f;
    private readonly float inverseTotalTime = 1f;

    private readonly Action onStart = delegate { };
    private readonly Action<float> onStep = delegate { };
    private readonly Action onEnd = delegate { };

    public bool active = false;

    public EventTimer(float totalTime, Action onStart = null, Action<float> onStep = null, Action onEnd = null)
    {
        inverseTotalTime = 1f / totalTime;

        if (onStart != null) this.onStart = onStart;
        if (onStep != null) this.onStep = onStep;
        if (onEnd != null) this.onEnd = onEnd;
    }

    public void Start()
    {
        active = true;
        /*currentTime =*/ progress = 0f;
        onStart();
    }

    public void Step()
    {
        // Safeguard
        if (!active) return;

        progress += Time.deltaTime * inverseTotalTime;
        onStep(progress);

        if (progress > 1f)
            Stop();
    }

    public void Stop()
    {
        active = false;
        onEnd();
    }
}
