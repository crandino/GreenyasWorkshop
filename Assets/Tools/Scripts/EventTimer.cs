using System;
using UnityEngine;

public class EventTimer
{
    public float Progress { private set; get; }
    private readonly float inverseTotalTime = 1f;

    //private readonly Action onStart = delegate { };
    //private readonly Action<float> onStep = delegate { };
    //private readonly Action onEnd = delegate { };

    public bool Active => Progress < 1.0f;

    public EventTimer(float totalTime /*Action onStart = null, Action<float> onStep = null, Action onEnd = null*/)
    {
        inverseTotalTime = 1f / totalTime;

        //if (onStart != null) this.onStart = onStart;
        //if (onStep != null) this.onStep = onStep;
        //if (onEnd != null) this.onEnd = onEnd;
    }

    //public void Start()
    //{
    //    active = true;
    //    progress = 0f;
    //    //onStart();
    //}

    public void Step()
    {
        // Safeguard
        //if (!active) return;

        Progress += Time.deltaTime * inverseTotalTime;
        //onStep(progress);

        //Debug.Log(progress);

        //if (progress > 1f)
        //    Stop();
    }

    //public void Stop()
    //{
    //    active = false;
    //    onEnd();
    //}
}
