using System.Collections;
using UnityEngine;


[RequireComponent(typeof(MeshRenderer))]
public class PathHighligther : MonoBehaviour
{
    [SerializeField]
    new private MeshRenderer renderer;

    [SerializeField]
    private float time = 1.0f;

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

    public IEnumerator UpdateHighlight(/*Color color, Action onHighlightEnds*/)
    {
        Debug.Log("Nobody loves me!");

        const float highlightSpeed = 0.866f; 

        float currentTime = 0;
        float inverseTime = 1f / (segmentLength / highlightSpeed);

        float normalizedTime = currentTime * inverseTime;

        float GetNormalizedTime(bool direction) => direction ? normalizedTime : 1f - normalizedTime;

        //renderer.material.SetColor(playerColorID, color);

        while (normalizedTime < 1f)
        {
            renderer.material.SetFloat(pathProgressID, GetNormalizedTime(direction));
            currentTime += Time.deltaTime;
            normalizedTime = currentTime * inverseTime;
            yield return null;
        }

        renderer.material.SetFloat(pathProgressID, Mathf.Clamp01(GetNormalizedTime(direction)));
        //onHighlightEnds();
    }

    private void Reset()
    {
        renderer = GetComponent<MeshRenderer>();
    }


}
