using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

namespace Greenyas.Hexagon
{
    [CreateAssetMenu(fileName = "DebugOptions", menuName = "ScriptableObjects/HexagonDebugVisualization", order = 1)]
    public class DebugOptions : ScriptableObject
    {
        [SerializeField] public bool showHexagonMapCoordinates;
        [SerializeField] public bool showTilePaths;
        [SerializeField] public bool showTileConnections;

        private static DebugOptions instance;

        public static bool ShowHexagonCoord => (instance ?? FindDebugOptionSO()).showHexagonMapCoordinates;
        public static bool ShowPaths => (instance ?? FindDebugOptionSO()).showTilePaths;
        public static bool ShowConnections => (instance ?? FindDebugOptionSO()).showTileConnections;

        private static DebugOptions FindDebugOptionSO()
        {
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(DebugOptions));  //FindAssets uses tags check documentation for more info
            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            instance = AssetDatabase.LoadAssetAtPath<DebugOptions>(path);
            return instance;
        }
    }
}
