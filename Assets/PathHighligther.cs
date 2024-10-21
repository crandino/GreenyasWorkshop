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

    private readonly static int backgroundPlayerColorID = Shader.PropertyToID("_BackgroundColor");
    private readonly static int forwardPlayerColorID = Shader.PropertyToID("_ForwardForegroundColor");
    private readonly static int backwardPlayerColorID = Shader.PropertyToID("_BackwardForegroundColor");

    private readonly static int forwardPathProgressID = Shader.PropertyToID("_ForwardPathProgress");
    private readonly static int backwardPathProgressID = Shader.PropertyToID("_BackwardPathProgress");

    private bool? forwardDirection = null;

    const float PROPAGATION_SPEED = 0.866f * 2;

    private Material ColorPropagator
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
            if (ColorPropagator.GetFloat(forwardPathProgressID) > 0.5f)
                return ColorPropagator.GetColor(forwardPlayerColorID);
            return ColorPropagator.GetColor(backwardPlayerColorID);
        }
    }   

    public bool PreHighlight(Color newColor, bool? forwardDir = true)
    {
        if (timer != null && !timer.IsCompleted)
            return false;

        timer = new(segmentLength / PROPAGATION_SPEED);
        this.forwardDirection = forwardDir;        

        Color currentColor = CurrentColor;
        ColorPropagator.SetColor(backgroundPlayerColorID, currentColor);

        if(forwardDir.HasValue)
        {
            if (forwardDir.Value)
            {
                ColorPropagator.SetColor(forwardPlayerColorID, newColor);
                ColorPropagator.SetColor(backwardPlayerColorID, currentColor);
            }
            else
            {
                ColorPropagator.SetColor(forwardPlayerColorID, currentColor);
                ColorPropagator.SetColor(backwardPlayerColorID, newColor);
            }
        }
        else
        {
            ColorPropagator.SetColor(forwardPlayerColorID, newColor);
            ColorPropagator.SetColor(backwardPlayerColorID, newColor);
        }

        ColorPropagator.SetFloat(forwardPathProgressID, 0f);
        ColorPropagator.SetFloat(backwardPathProgressID, 0f);
        return true;

        // TODO: 
        /*
         *  Poner orden. El bool? cambiarlo por un enum (no queda claro)
         *  La gestión de los paths y el historial de caminos, automatizarlo más
         *  Crear extensiones para modificar varios parámetros a la vez en los renderers
         *  Utilizar el TimerNormalized es la mejor opción
         *  Crear mejores namespaces
         */
    }

    public void Highlight(Color color)
    {
        PreHighlight(color);
        ColorPropagator.SetFloat(forwardPathProgressID, 1f);
    }

    //public void Unhighlight(Color color)
    //{
    //    renderer.material.SetFloat(pathProgressID, 0.0f);
    //    renderer.material.SetColor(playerColorID, color);
    //}

    private NormalizedTimer timer = null;

    public IEnumerator UpdateHighlight()
    {
        int[] progressID = forwardDirection.HasValue ? ( forwardDirection.Value ? new[] { forwardPathProgressID } : new[] { backwardPathProgressID }) : new int[] { forwardPathProgressID, backwardPathProgressID };

        while (!timer.IsCompleted)
        {
            foreach(var p in progressID)
                renderer.material.SetFloat(p, timer.Time);
            timer.Step(Time.deltaTime);
            yield return null;
        }

        foreach (var p in progressID)
            renderer.material.SetFloat(p, Mathf.Clamp01(timer.Time));
    }

#if UNITY_EDITOR
    private void Reset()
    {
        renderer = GetComponent<MeshRenderer>();
    } 
#endif
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
