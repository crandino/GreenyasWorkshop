using System;

public class NormalizedTimer
{
    private readonly float inverseTime;
    private float currentTime = 0f;
    private float NormalizedTime => currentTime * inverseTime;

    private readonly Func<float> GetNormalized;

    public float Time => GetNormalized();
    public bool IsCompleted => GetNormalizedTime() >= 1.0f;

    public NormalizedTimer(float totalTime, bool inverse = false)
    {
        inverseTime = 1f / totalTime;
        GetNormalized = inverse ? GetInverseNormalizedTime : GetNormalizedTime;
    }

    public void Reset()
    {
        currentTime = 0f;
    }

    private float GetNormalizedTime() => NormalizedTime;
    private float GetInverseNormalizedTime() => 1f - (NormalizedTime);

    public void Step(float deltaTime)
    {
        currentTime += deltaTime;
    }
}