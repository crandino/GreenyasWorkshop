using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

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
        //string guid = AssetDatabase.CreateFolder("Assets/My Folder", "My Another Folder");
        //string newFolderPath = AssetDatabase.GUIDToAssetPath(guid);
        //Debug.Log(newFolderPath);

        //// Create a simple material asset in the created folder
        //Material material = new Material(Shader.Find("Specular"));
        //string newAssetPath = newFolderPath + "/MyMaterial.mat";
        //AssetDatabase.CreateAsset(material, newAssetPath);
        //Debug.Log(AssetDatabase.GetAssetPath(material));
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
