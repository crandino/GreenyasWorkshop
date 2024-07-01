#if UNITY_EDITOR
using System.Linq;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Splines;

public class UVPathMapper : MonoBehaviour
{
    [SerializeField]
    private MeshFilter meshFilter;

    [SerializeField]
    private SplineContainer splineContainer;

    private void Reset()
    {
        meshFilter = GetComponent<MeshFilter>();
        splineContainer = GetComponent<SplineContainer>();
    }

    [ContextMenu("Reorganize UVs")]
    private void Reorganize()
    {
        string[] guids = AssetDatabase.FindAssets(meshFilter.sharedMesh.name);
        string path = AssetDatabase.GUIDToAssetPath(guids[0]);

        string newMeshName = meshFilter.sharedMesh.name + "_customUV.asset";

        Mesh mesh = UnityEngine.ProBuilder.MeshUtility.DeepCopy(meshFilter.sharedMesh);

        Vector3[] vertices = mesh.vertices;
        Vector2[] newUVs = new Vector2[vertices.Length];

        float maxHeight = vertices.Max(v => v.y);
        float minHeight = vertices.Min(v => v.y);

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 worldVertex = transform.localToWorldMatrix * new Vector4(vertices[i].x, vertices[i].y, vertices[i].z, 1);

            SplineUtility.GetNearestPoint(splineContainer.Spline, vertices[i], out float3 v, out float t, SplineUtility.PickResolutionMax);
            float normalizedHeight = Mathf.InverseLerp(minHeight, maxHeight, vertices[i].y);
            newUVs[i] = new Vector2(Mathf.Clamp01(t), normalizedHeight * 0.5f);
        }

        mesh.SetUVs(0, newUVs);
        AssetDatabase.CreateAsset(mesh, path.Substring(0, path.LastIndexOf(".asset")) + "_customUV.asset");
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = mesh;
    }
} 
#endif
