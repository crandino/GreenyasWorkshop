using Greenyas.Hexagon;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public partial class TilePlacer
{
    private class DragAndDropManipulator : PointerManipulator
    {
        string tilePrefabPath = string.Empty;

        public DragAndDropManipulator(TilePrefabLabel pathLabel)
        {
            target = pathLabel;
            tilePrefabPath = pathLabel.AssetPrefab;
        }

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<PointerDownEvent>(OnPointerDown);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<PointerDownEvent>(OnPointerDown);
        }

        void OnPointerDown(PointerDownEvent _)
        {
            GetWindow(typeof(SceneView)).rootVisualElement.RegisterCallback<DragPerformEvent>(OnDragScenePerformed);

            DragAndDrop.PrepareStartDrag();
            DragAndDrop.StartDrag("Dragging");
            DragAndDrop.objectReferences = new Object[] { AssetDatabase.LoadAssetAtPath<GameObject>(tilePrefabPath) };
        }     

        void OnDragScenePerformed(DragPerformEvent evt)
        {
            DragAndDrop.AcceptDrag();
            GetWindow(typeof(SceneView)).rootVisualElement.UnregisterCallback<DragPerformEvent>(OnDragScenePerformed);

            GameObject tile = HandleUtility.PickGameObject(evt.localMousePosition, false);
            CubeCoord coord = HexTools.GetNearestCubeCoord(tile.transform.position);
            tile.transform.position = HexTools.GetCartesianWorldPos(coord);
        }       
    }
}