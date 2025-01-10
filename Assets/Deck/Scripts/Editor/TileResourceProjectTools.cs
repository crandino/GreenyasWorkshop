using HexaLinks.Tile;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class TileResourceProjectTools
{
    private const string ICON_FOLDER_NAME = "Icon";
    private const string PREFABS_FOLDER_NAME = "Prefabs";

    [MenuItem("Assets/Create Tile Resource")]
    static void CreateResource()
    {
        string currentSelectedPpath = EditorShortcuts.GetSelectedPathOrFallback();
        string iconFolderPath = Path.Combine(currentSelectedPpath, ICON_FOLDER_NAME);
        string[] files = Directory.GetFiles(iconFolderPath);
        Sprite icon = AssetDatabase.LoadAllAssetsAtPath(files[0]).OfType<Sprite>().First();

        string prefabFolderPath = Path.Combine(currentSelectedPpath, PREFABS_FOLDER_NAME);
        files = Directory.GetFiles(prefabFolderPath);
        Tile prefab = AssetDatabase.LoadAllAssetsAtPath(files[0]).OfType<Tile>().First();

        TileResource sourceInstance = TileResource.Create(icon, prefab);
        string folderName = currentSelectedPpath.Split(Path.AltDirectorySeparatorChar).Last();
        string assetPath = Path.Combine(currentSelectedPpath, $"{folderName}_R.asset");
        AssetDatabase.CreateAsset(sourceInstance, assetPath);
    }

}