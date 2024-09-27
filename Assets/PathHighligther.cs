using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshRenderer))]
public class PathHighligther : MonoBehaviour
{
    [SerializeField]
    new private MeshRenderer renderer;

    private static int playerColorID = Shader.PropertyToID("_PathSelectionColor");
    private static int pathProgressID = Shader.PropertyToID("_PathProgress");

    public void Highlight(Color color)
    {
        renderer.material.SetFloat(pathProgressID, 1.0f);
        renderer.material.SetColor(playerColorID, color);

       // renderer.SetPropertyBlock(materialPropertyBlock);
    }

    public void Unhighlight(Color color)
    {
        renderer.material.SetFloat(pathProgressID, 0.0f);
        renderer.material.SetColor(playerColorID, color);
    }

    private void Reset()
    {
        renderer = GetComponent<MeshRenderer>();
    }
}
