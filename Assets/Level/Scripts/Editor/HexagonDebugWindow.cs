using System.Collections;
using TMPro.EditorUtilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Greenyas.Hexagon
{
    public class HexagonDebugWindow : EditorWindow
    {
        [SerializeField]
        private DebugOptions debugOptions;

        [MenuItem("Window/Hexagon Debug Options")]
        public static void Init()
        {
            GetWindow(typeof(HexagonDebugWindow));
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
