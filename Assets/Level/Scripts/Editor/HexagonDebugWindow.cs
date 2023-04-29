using UnityEditor;
using UnityEngine;

namespace Greenyas.Hexagon
{
    public class HexagonDebugWindow : EditorWindow
    {
        [SerializeField]
        private DebugOptions debugOptions;

        [MenuItem("Window/Greenyas/Hexagon Debug Options")]
        public static void Init()
        {
            GetWindow<HexagonDebugWindow>("Debug Options");
        }

        void OnGUI()
        {
            EditorGUILayout.LabelField("Debug Visualization");
            EditorGUI.indentLevel++;
            ShowToogle("Map Coordinates",ref debugOptions.showHexagonMapCoordinates);
            ShowToogle("Tile Paths", ref debugOptions.showTilePaths);
            ShowToogle("Tile Connections",ref debugOptions.showTileConnections);
            EditorGUI.indentLevel--;
        }

        private void ShowToogle(string label, ref bool option)
        {
            option = EditorGUILayout.Toggle(label, option);
        }
    } 
}
