using Greenyas.Hexagon;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public partial class TilePlacer
{
    private static VisualElement SceneView => GetWindow(typeof(SceneView)).rootVisualElement;

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
            SceneView.RegisterCallback<DragPerformEvent>(OnDragScenePerformed);

            DragAndDrop.PrepareStartDrag();
            DragAndDrop.StartDrag("Dragging");
            //Selection.activeGameObject = Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>(tilePrefabPath));
            DragAndDrop.objectReferences = new Object[] { Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>(tilePrefabPath)) };
        }

        void OnDragScenePerformed(DragPerformEvent evt)
        {
            DragAndDrop.AcceptDrag();
            SceneView.UnregisterCallback<DragPerformEvent>(OnDragScenePerformed);

            GameObject tile = GetCurrentDropGameObject(evt.originalMousePosition);
            tile.transform.position = HexTools.GetGridCartesianWorldPos(tile.transform.position);
        }

        private GameObject GetCurrentDropGameObject(Vector2 mousePosition)
        {
            GameObject gameObject = HandleUtility.PickGameObject(mousePosition, true);
            return gameObject.transform.GetTransformRoot().gameObject;
        }
    }
}