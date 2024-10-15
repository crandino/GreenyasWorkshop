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

    private Material PathFiller
    {
        get
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                Material mat = new(renderer.sharedMaterial);
                renderer.material = mat;
                return mat;
            }
#endif

            return renderer.material;
        }
    }

    private Color CurrentColor
    {
        get
        {
            if (PathFiller.GetFloat(pathProgressID) > 0.5f)
                return PathFiller.GetColor(foregroundPlayerColorID);
            return PathFiller.GetColor(backgroundPlayerColorID);
        }
    }   

    public void Configure(Color newColor, bool direction = true)
    {
        this.direction = direction;

        if (direction)
        {
            PathFiller.SetColor(backgroundPlayerColorID, CurrentColor);
            PathFiller.SetColor(foregroundPlayerColorID, newColor);
            PathFiller.SetFloat(pathProgressID, 0f);
        }
        else
        {
            PathFiller.SetColor(foregroundPlayerColorID, CurrentColor);
            PathFiller.SetColor(backgroundPlayerColorID, newColor);
            PathFiller.SetFloat(pathProgressID, 1f);
        }
    }

    public void Highlight(Color color)
    {
        Configure(color);
        PathFiller.SetFloat(pathProgressID, 1f);
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
