using System.Collections;
using TMPro.EditorUtilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Greenyas.Hexagon
{
    public partial class HexagonDebugWindow : EditorWindow
    {
        private static EditorWindow window;

        [MenuItem("Window/Hexagon Debug Options")]
        public static void Init()
        {
            window = GetWindow(typeof(HexagonDebugWindow));
            TMP_EditorCoroutine.StartCoroutine(InitManipulator());
        }

        void OnGUI()
        {
            EditorGUILayout.LabelField("Debug Visualization");
            EditorGUI.indentLevel++;
            ShowToogle("Map Coordinates", ref DebugOptions.showHexagonMapCoordinates);
            ShowToogle("Tile Paths", ref DebugOptions.showTilePaths);
            ShowToogle("Tile Connections", ref DebugOptions.showTileConnections);
            EditorGUI.indentLevel--;

            //Tenet();
        }

        private static DragAndDropManipulator manipulator;

        private void Tenet()
        {
            UnityEngine.Object go = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Level/Tiles/Starters/DoubleStarter/Prefabs/DoubleStarter.prefab", typeof(GameObject));
            EditorGUILayout.ObjectField("Starter", go, typeof(GameObject), false);
            //DragAndDrop.PrepareStartDrag();
            //DragAndDrop.AcceptDrag();
            //for (int i = 0; i < DragAndDrop.objectReferences.Length; i++)
            //{
            //    Debug.Log(DragAndDrop.objectReferences[i]);
            //}
        }

        private void ShowToogle(string label, ref bool option)
        {
            option = EditorGUILayout.Toggle(label, option);
        }

        //private static IEnumerator InitManipulator()
        //{
        //    yield return new WaitForSeconds(0.25f);
        //    manipulator = new DragAndDropManipulator(window);
        //}

        private static IEnumerator InitManipulator()
        {
            yield return new WaitForSeconds(0.25f);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Level/Tiles/Starters/DoubleStarter/Prefabs/DoubleStarter.prefab");

            var box = new VisualElement();
            box.style.backgroundColor = Color.red;
            box.style.flexGrow = 1f;

            window.rootVisualElement.RegisterCallback<MouseDownEvent>(evt =>
            {
                DragAndDrop.PrepareStartDrag();
                DragAndDrop.StartDrag("Dragging");
                DragAndDrop.objectReferences = new Object[] { prefab };
            });

            window.rootVisualElement.RegisterCallback<DragUpdatedEvent>(evt =>
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Move;
            });

            //rootVisualElement.Add(box);
        }

        private void OnDisable()
        {
            //manipulator.target.RemoveManipulator(manipulator);
        }
    } 
}
