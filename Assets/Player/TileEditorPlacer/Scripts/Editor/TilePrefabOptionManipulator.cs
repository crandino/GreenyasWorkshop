using Hexalinks.Tile;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class TilePrefabOptionManipulator : Manipulator
{
    public TilePrefabOptionManipulator(TilePrefabOption tileOption)
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

        TileEditorManipulator.Set(Object.Instantiate(((TilePrefabOption)target).TilePrefab, Vector3.zero, Quaternion.identity), target);
    }

    //public void CancelPlacement()
    //{
    //    if (instantiatedTile != null)
    //        Object.DestroyImmediate(instantiatedTile.gameObject);

    //    FinishPlacement();
    //}

    //private void AcceptPlacement()
    //{
    //    tilePos.AttachToGrid();
    //    GameObject.FindAnyObjectByType<HexMap>().AddTile(instantiatedTile);

    //    FinishPlacement();
    //}

    //private void FinishPlacement()
    //{ 
    //    Object.DestroyImmediate(tile.gameObject);
    //    target.style.backgroundColor = manipulationInactiveColor;
    //    target.UnregisterCallback<KeyDownEvent>(ManipulateTile);
    //    instantiatedTile = null;
    //}

    //private void ChangeOwner(PlayerOwnership.Ownership owner)
    //{
    //    PlayerOwnership[] ownerships;
    //    //if( (ownerships = instantiatedTile.GetComponentsInChildren<InitialPlayerOwnership>()) || (ownerships = instantiatedTile.GetComponentsInChildren<PlayerOwnership>()))
    //    //    Array.ForEach(ownerships, o => o.InstantOwnerChange(owner));
    //    //if (ownerships != null)
    //}

    //private void ManipulateTile(KeyDownEvent evt)
    //{
    //    evt.StopImmediatePropagation();

    //    switch (evt.keyCode)
    //    {
    //        case KeyCode.Return:
    //            AcceptPlacement();
    //            break;
    //        case KeyCode.Escape:
    //            CancelPlacement();
    //            break;
    //        case KeyCode.A:
    //            tilePos.MoveLeft();
    //            break;
    //        case KeyCode.D:
    //            tilePos.MoveRight();
    //            break;
    //        case KeyCode.E:
    //            tileRot.RotateClockwise();
    //            break;
    //        case KeyCode.Q:
    //            tileRot.RotateCounterClockwise();
    //            break;
    //        case KeyCode.S:
    //            tilePos.MoveDown();
    //            break;
    //        case KeyCode.W:
    //            tilePos.MoveUp();
    //            break;
    //        case KeyCode.Alpha0:
    //            ChangeOwner(PlayerOwnership.Ownership.None);
    //            break;
    //        case KeyCode.Alpha1:
    //            ChangeOwner(PlayerOwnership.Ownership.PlayerOne);
    //            break;
    //        case KeyCode.Alpha2:
    //            ChangeOwner(PlayerOwnership.Ownership.PlayerTwo);
    //            break;
    //        default:
    //            break;
    //    }
    //}
}
