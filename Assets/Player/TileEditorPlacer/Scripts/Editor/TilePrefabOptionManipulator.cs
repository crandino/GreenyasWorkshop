using UnityEngine;
using UnityEngine.UIElements;

public class TilePrefabOptionManipulator : Manipulator
{
    public TilePrefabOptionManipulator(TileResourceOption tileOption)
    {
        target = tileOption;        
    }

    protected override void RegisterCallbacksOnTarget()
    {
        target.RegisterCallback<PointerDownEvent>(StartPlacement);
    }

    protected override void UnregisterCallbacksFromTarget()
    {
        target.UnregisterCallback<PointerDownEvent>(StartPlacement);
    }

    private void StartPlacement(PointerDownEvent _)
    {
        if (!TileEditorManipulator.IsAvailable())
            return;

        TileEditorManipulator.Set(Object.Instantiate(((TileResourceOption)target).TileResource.Prefab, Vector3.zero, Quaternion.identity), target);
    }
}
