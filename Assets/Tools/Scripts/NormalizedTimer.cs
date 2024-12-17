using System;
using System.Collections.Generic;
using UnityEngine;

public class NormalizedTimer
{
    private readonly float inverseTime;
    private float currentTime = 0f;
    public float NormalizedTime => Mathf.Clamp01(currentTime * inverseTime);

    public float Time => NormalizedTime;
    public bool IsCompleted => GetNormalizedTime() >= 1.0f;

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

    public NormalizedTimer(float totalTime, float initialTime = 0f)
    {
        inverseTime = 1f / totalTime;
        currentTime = initialTime / inverseTime;
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

    private float GetNormalizedTime() => NormalizedTime;

    public void Step(float deltaTime)
    {
        currentTime += deltaTime;

        for (int i = 0; i < events.Count; i++)
        {
            if (!events[i].triggered && NormalizedTime >= events[i].time)
                events[i].Call();
        }
    }
}