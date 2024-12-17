using System;
using System.Collections.Generic;
using UnityEngine;

public class NormalizedTimer
{
    private float currentInverseTime;
    private float targetInverseTime;

    private float velocity = 0f;

    private float currentTime = 0f;
    public float NormalizedTime => Mathf.Clamp01(currentTime * currentInverseTime);

    public float Time => NormalizedTime;
    //public bool IsCompleted => GetNormalizedTime() >= 1.0f;

    public float TotalTime
    {
        set
        {
            targetInverseTime = 1f / value;
        }
    }

    private class TimeEvent
    {
        public bool triggered;
        public float time;
        public Action callback;

        public void Call()
        {
            triggered = true;
            callback();
        }

        public void ResetCallbacks()
        {
            triggered = false;
        }
    }

    private readonly List<TimeEvent> events = new List<TimeEvent>();

    public NormalizedTimer(float totalTime)
    {
        TotalTime = totalTime;
        currentTime = 0.0f;
        currentInverseTime = 1f;
    }

    public void AddEvent(float normalizedTime, Action timeCallback)
    {
        events.Add(new TimeEvent
        {
            time = normalizedTime,
            callback = timeCallback,
            triggered = false
        });
    }

    public void Reset()
    {
        currentTime = 0f;
        for (int i = 0; i < events.Count; i++)
            events[i].ResetCallbacks();
    }

    public void Step(float deltaTime)
    {
        currentInverseTime = Mathf.SmoothDamp(currentInverseTime, targetInverseTime, ref velocity, 1f);
        currentTime += deltaTime;

        for (int i = 0; i < events.Count; i++)
        {
            if (!events[i].triggered && NormalizedTime >= events[i].time)
                events[i].Call();
        }
    }
}