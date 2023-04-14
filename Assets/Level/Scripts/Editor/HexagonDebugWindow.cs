using UnityEditor;
using UnityEngine;

namespace Greenyas.Hexagon
{
    class HexagonDebugWindow : EditorWindow
    {
        [MenuItem("Window/Hexagon Debug Options")]
        public static void Init()
        {
            GetWindow(typeof(HexagonDebugWindow));            
        }

        void OnGUI()
        {
            EditorGUILayout.LabelField("Debug Visualization");
            EditorGUI.indentLevel++;
            ShowToogle("Map Coordinates", ref DebugOptions.showHexagonMapCoordinates);
            ShowToogle("Tile Paths", ref DebugOptions.showTilePaths);
            ShowToogle("Tile Connections", ref DebugOptions.showTileConnections);
            EditorGUI.indentLevel--;
        }

        private void ShowToogle(string label, ref bool option)
        {
            option = EditorGUILayout.Toggle(label, option);
        }
    } 
}
