#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Greenyas.Hexagon
{
    [CreateAssetMenu(fileName = "DebugOptions", menuName = "Hexagon/DebugVisualization", order = 1)]
    public class DebugOptions : ScriptableSingleton<DebugOptions>
    {
        public bool showHexagonMapCoordinates;
        public bool showTilePaths;
        public bool showTileConnections;     
    }
} 
#endif
