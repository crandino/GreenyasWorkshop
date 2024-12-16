using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class ExportMesh : MonoBehaviour
{
    [SerializeField]
    private MeshFilter meshFilter;

    [ContextMenu("Export")]
    private void Export()
    {
        Mesh mesh = UnityEngine.ProBuilder.MeshUtility.DeepCopy(meshFilter.sharedMesh);
        AssetDatabase.CreateAsset(mesh, "Assets/Test.asset");
    }

    private void Reset()
    {
        meshFilter = gameObject.GetComponent<MeshFilter>();
    }
} 
#endif
