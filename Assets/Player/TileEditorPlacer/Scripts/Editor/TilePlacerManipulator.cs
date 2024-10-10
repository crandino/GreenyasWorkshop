using Hexalinks.Tile;
using UnityEngine;
using UnityEngine.UIElements;

public class TilePlacerManipulator : Manipulator
{
    // Cuando una pieza se quiera colocar sobre otra, no se puede poner.
    // Crear ese modo rápido de recolocación de piezas desde este manipulador

    private Tile instantiatedTile = null;

    private EditorTilePosition tilePos;
    private EditorTileRotation tileRot;

    private readonly static Color manipulationActiveColor = new(1f, 0.984f, 0f, 0.5f);
    private readonly static Color manipulationInactiveColor = new(.5f, .5f, .5f, 1f);

    public TilePlacerManipulator(TilePrefabOption tileOption)
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
        if (instantiatedTile != null)
            return;

        instantiatedTile = Object.Instantiate(((TilePrefabOption)target).TilePrefab, Vector3.zero, Quaternion.identity);
        tilePos = new EditorTilePosition(instantiatedTile.Coordinates, TilePosition.PositionMode.HOVER);
        tileRot = new EditorTileRotation(instantiatedTile.Coordinates);

        target.RegisterCallback<KeyDownEvent>(ManipulateTile);
        target.style.backgroundColor = manipulationActiveColor;
    }

    public void CancelPlacement()
    {
        if (instantiatedTile != null)
            Object.DestroyImmediate(instantiatedTile.gameObject);

        FinishPlacement();
    }

    private void AcceptPlacement()
    {
        tilePos.AttachToGrid();
        GameObject.FindAnyObjectByType<HexMap>().AddTile(instantiatedTile);

        FinishPlacement();
    }

    private void FinishPlacement()
    {
        target.style.backgroundColor = manipulationInactiveColor;
        target.UnregisterCallback<KeyDownEvent>(ManipulateTile);
        instantiatedTile = null;
    }

    private void ChangeOwner(PlayerOwnership.Ownership owner)
    {
        var ownership = instantiatedTile.GetComponentInChildren<InitialPlayerOwnership>();
        if(ownership != null)
            ownership.InstantOwnerChange(owner);
    }

    private void ManipulateTile(KeyDownEvent evt)
    {
        evt.StopImmediatePropagation();

        switch (evt.keyCode)
        {
            case KeyCode.Return:
                AcceptPlacement();
                break;
            case KeyCode.Escape:
                CancelPlacement();
                break;
            case KeyCode.A:
                tilePos.MoveLeft();
                break;
            case KeyCode.D:
                tilePos.MoveRight();
                break;
            case KeyCode.E:
                tileRot.RotateClockwise();
                break;
            case KeyCode.Q:
                tileRot.RotateCounterClockwise();
                break;
            case KeyCode.S:
                tilePos.MoveDown();
                break;
            case KeyCode.W:
                tilePos.MoveUp();
                break;
            case KeyCode.Alpha0:
                ChangeOwner(PlayerOwnership.Ownership.None);
                break;
            case KeyCode.Alpha1:
                ChangeOwner(PlayerOwnership.Ownership.PlayerOne);
                break;
            case KeyCode.Alpha2:
                ChangeOwner(PlayerOwnership.Ownership.PlayerTwo);
                break;
            default:
                break;
        }
    }
}
