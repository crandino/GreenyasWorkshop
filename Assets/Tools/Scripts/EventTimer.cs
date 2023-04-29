using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTimer
{
    private float progress = 0f;

    private float currentTime = 0f;
    private float inverseTotalTime = 1f;

    private Action onStart = delegate { };
    private Action<float> onStep = delegate { };
    private Action onEnd = delegate { };

    public EventTimer(float totalTime, Action onStart = null, Action<float> onStep = null, Action onEnd = null)
    {
        inverseTotalTime = 1f / totalTime;

        if (onStart != null) this.onStart = onStart;
        if (onStep != null) this.onStep = onStep;
        if (onEnd != null) this.onEnd = onEnd;
    }

    public void Start()
    {
        progress = currentTime = 0f;
        onStart();
    }

    public void Step()
    {
        currentTime += Time.deltaTime;
        progress += currentTime * inverseTotalTime;
        onStep(progress);

        if (progress > 1f)
            Stop();
    }

    public void Stop()
    {
        onEnd();
    }
}
