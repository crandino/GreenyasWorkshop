using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public partial class TilePlacer
{
    private class DragAndDropManipulator : PointerManipulator
    {
        // The Label in the window that shows the stored asset, if any.
        //Label dropLabel;
        // The stored asset object, if any.
        string tilePrefabPath = string.Empty;
        //// The path of the stored asset, or the empty string if there isn't one.
        //string assetPath = string.Empty;

        public DragAndDropManipulator(PrefabPathLabel pathLabel)
        {
            // The target of the manipulator, the object to which to register all callbacks, is the drop area.
            //target = root.Q<VisualElement>(className: "drop-area");
            target = pathLabel;
            tilePrefabPath = pathLabel.AssetPrefab;
            //root = target.parent;
            //dropLabel = root.Q<Label>(className: "drop-area__label");
        }

        protected override void RegisterCallbacksOnTarget()
        {
            // Register a callback when the user presses the pointer down.
            target.RegisterCallback<PointerDownEvent>(OnPointerDown);
            // Register callbacks for various stages in the drag process.
            target.RegisterCallback<DragUpdatedEvent>(OnDragUpdate);
            //target.RegisterCallback<DragEnterEvent>(OnDragEnter);
            //target.RegisterCallback<DragLeaveEvent>(OnDragLeave);
            //target.RegisterCallback<DragPerformEvent>(OnDragPerform);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            // Unregister all callbacks that you registered in RegisterCallbacksOnTarget().
            target.UnregisterCallback<PointerDownEvent>(OnPointerDown);
            target.UnregisterCallback<DragUpdatedEvent>(OnDragUpdate);
            //target.UnregisterCallback<DragEnterEvent>(OnDragEnter);
            //target.UnregisterCallback<DragLeaveEvent>(OnDragLeave);
            //target.UnregisterCallback<DragPerformEvent>(OnDragPerform);
        }

        // This method runs when a user presses a pointer down on the drop area.
        void OnPointerDown(PointerDownEvent _)
        {
            DragAndDrop.PrepareStartDrag();
            DragAndDrop.StartDrag("Dragging");
            DragAndDrop.objectReferences = new Object[] { AssetDatabase.LoadAssetAtPath<GameObject>(tilePrefabPath) };
        }

        // This method runs if a user brings the pointer over the target while a drag is in progress.
        //void OnDragEnter(DragEnterEvent _)
        //{
        //    // Get the name of the object the user is dragging.
        //    var draggedName = string.Empty;
        //    if (DragAndDrop.paths.Length > 0)
        //    {
        //        assetPath = DragAndDrop.paths[0];
        //        var splitPath = assetPath.Split('/');
        //        draggedName = splitPath[splitPath.Length - 1];
        //    }
        //    else if (DragAndDrop.objectReferences.Length > 0)
        //    {
        //        draggedName = DragAndDrop.objectReferences[0].name;
        //    }

        //    // Change the appearance of the drop area if the user drags something over the drop area and holds it
        //    // there.
        //    dropLabel.text = $"Dropping '{draggedName}'...";
        //    target.AddToClassList("drop-area--dropping");
        //}

        // This method runs if a user makes the pointer leave the bounds of the target while a drag is in progress.
        //void OnDragLeave(DragLeaveEvent _)
        //{
        //    assetPath = string.Empty;
        //    droppedObject = null;
        //    dropLabel.text = "Drag an asset here...";
        //    target.RemoveFromClassList("drop-area--dropping");
        //}

        // This method runs every frame while a drag is in progress.
        void OnDragUpdate(DragUpdatedEvent _)
        {
            DragAndDrop.visualMode = DragAndDropVisualMode.Move;
        }

        // This method runs when a user drops a dragged object onto the target.
        //void OnDragPerform(DragPerformEvent _)
        //{
        //    // Set droppedObject and draggedName fields to refer to dragged object.
        //    droppedObject = DragAndDrop.objectReferences[0];
        //    string draggedName;
        //    if (assetPath != string.Empty)
        //    {
        //        var splitPath = assetPath.Split('/');
        //        draggedName = splitPath[splitPath.Length - 1];
        //    }
        //    else
        //    {
        //        draggedName = droppedObject.name;
        //    }

        //    // Visually update target to indicate that it now stores an asset.
        //    //dropLabel.text = $"Containing '{draggedName}'...\n\n" *
        //    //    $"(You can also drag from here)";
        //    //target.RemoveFromClassList("drop-area--dropping");
        //}
    }
}