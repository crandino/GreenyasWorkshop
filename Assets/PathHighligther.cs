using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class PathHighligther : MonoBehaviour
{
    [SerializeField]
    new private MeshRenderer renderer;

    [SerializeField]
    private float segmentLength = 0.866f;

    private readonly static int foregroundPlayerColorID = Shader.PropertyToID("_ForegroundColor");
    private readonly static int backgroundPlayerColorID = Shader.PropertyToID("_BackgroundColor");
    private readonly static int pathProgressID = Shader.PropertyToID("_PathProgress");

    private bool direction = true;

    private Color CurrentColor
    {
        get
        {
            if (renderer.material.GetFloat(pathProgressID) > 0.5f)
                return renderer.material.GetColor(foregroundPlayerColorID);
            return renderer.material.GetColor(backgroundPlayerColorID);
        }
    }   

    public void Configure(Color newColor, bool direction = true)
    {
        this.direction = direction;

        if(direction)
        {
            renderer.material.SetColor(backgroundPlayerColorID, CurrentColor);
            renderer.material.SetColor(foregroundPlayerColorID, newColor);
            renderer.material.SetFloat(pathProgressID, 0f);
        }
        else
        {
            renderer.material.SetColor(foregroundPlayerColorID, CurrentColor);
            renderer.material.SetColor(backgroundPlayerColorID, newColor);
            renderer.material.SetFloat(pathProgressID, 1f);
        }
    }

    public void Highlight(Color color)
    {
        Configure(color);
        renderer.material.SetFloat(pathProgressID, 1f);
    }


    //public void Unhighlight(Color color)
    //{
    //    renderer.material.SetFloat(pathProgressID, 0.0f);
    //    renderer.material.SetColor(playerColorID, color);
    //}

    public IEnumerator UpdateHighlight()
    {
        const float highlightSpeed = 0.866f * 2;

        NormalizedTimer timer = new(segmentLength / highlightSpeed, !direction);

        while (!timer.IsCompleted)
        {
            renderer.material.SetFloat(pathProgressID, timer.Time);
            timer.Step(Time.deltaTime);
            yield return null;
        }

        renderer.material.SetFloat(pathProgressID, Mathf.Clamp01(timer.Time));
    }

    private void Reset()
    {
        renderer = GetComponent<MeshRenderer>();
    }


}

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
