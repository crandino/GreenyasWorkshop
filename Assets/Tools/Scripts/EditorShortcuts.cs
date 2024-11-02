#if UNITY_EDITOR
using System.IO;
using UnityEditor;

public static class EditorShortcuts
{
    [MenuItem("Assets/Create Common Folders")]
    static void CreateFolder()
    {
        string currentSelectedPpath = GetSelectedPathOrFallback();

        AssetDatabase.CreateFolder(currentSelectedPpath, "Materials");
        AssetDatabase.CreateFolder(currentSelectedPpath, "Textures");
        AssetDatabase.CreateFolder(currentSelectedPpath, "Scripts");
        AssetDatabase.CreateFolder(currentSelectedPpath, "Prefabs");
    }

    public static string GetSelectedPathOrFallback()
    {
        string path = "Assets";

        foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
        {
            path = AssetDatabase.GetAssetPath(obj);
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                path = Path.GetDirectoryName(path);
                break;
            }
        }
        return path;
    }
} 
#endif
