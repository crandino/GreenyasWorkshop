#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Hexagon.Tile.Debug
{
    public class TileDebugOptions : ScriptableObject
    {
        [Header("Options")]

        public bool showNodes = true;
        public bool showSegments = true;
        public bool showConnections = true;

        private static TileDebugOptions instance = null;

        private const string FILE_PATH = "Assets/Tools/TileDebugger/Data/TileDebugger.asset";

        public static TileDebugOptions Instance
        {
            get
            {
                if (instance == null)
                    instance = AssetDatabase.LoadAssetAtPath<TileDebugOptions>(FILE_PATH) ?? CreateAsset();

                return instance;
            }
        }

        private static TileDebugOptions CreateAsset()
        {
            string directoryName = Path.GetDirectoryName(FILE_PATH);
            if (!Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName);

            TileDebugOptions obj = CreateInstance(typeof(TileDebugOptions)) as TileDebugOptions;
            AssetDatabase.CreateAsset(obj, FILE_PATH);
            return obj;
        }
    }
}

#endif


