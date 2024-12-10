using HexaLinks.Tile;
using HexaLinks.Ownership;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public static class TileEditorManipulator
{
    private readonly static HexMap hexMap;
    private readonly static Label selectedTileTextField;

    private static Tile tile = null;
    private static VisualElement callbackPlaceholder = null;

    private static EditorTilePosition tilePos;
    private static EditorTileRotation tileRot;

    public static bool IsAvailable(Tile tileToSet = null) => tile == null || tile != tileToSet;

    static TileEditorManipulator()
    {
        hexMap = GameObject.FindAnyObjectByType<HexMap>();
        selectedTileTextField = EditorWindow.GetWindow<TilePlacerWindow>().rootVisualElement.Q<Label>("SelectedTileName");
    }

    public static void Set(Tile editableTile, VisualElement callbackPlaceholderElement, TilePosition.PositionMode positionMode = TilePosition.PositionMode.HOVER)
    {
        tile = editableTile;

        callbackPlaceholder = callbackPlaceholderElement;
        callbackPlaceholder.RegisterCallback<KeyDownEvent>(ManipulateTile);

        tilePos = new EditorTilePosition(tile.Coordinates, positionMode);
        tileRot = new EditorTileRotation(tile.Coordinates);

        selectedTileTextField.text = tile.name;
    }

    public static void Unset()
    {
        selectedTileTextField.text = "None";
        callbackPlaceholder?.UnregisterCallback<KeyDownEvent>(ManipulateTile);
        
        Selection.activeObject = null;
        callbackPlaceholder = null;
        tile = null;
    }

    private static void ManipulateTile(KeyDownEvent evt)
    {
        evt.StopImmediatePropagation();

        switch (evt.keyCode)
        {
            case KeyCode.Return:
                DoPlacement();
                break;
            case KeyCode.Escape:
                UndoPlacement();
                break;
            case KeyCode.Delete:
                DestroyTile();
                break;
            case KeyCode.A:
                tilePos.MoveLeft();
                break;
            case KeyCode.D:
                tilePos.MoveRight();
                break;
            case KeyCode.E:
                if(tilePos.Editable)
                    tileRot.RotateClockwise();
                break;
            case KeyCode.Q:
                if (tilePos.Editable)
                    tileRot.RotateCounterClockwise();
                break;
            case KeyCode.S:
                tilePos.MoveDown();
                break;
            case KeyCode.W:
                tilePos.MoveUp();
                break;
            case KeyCode.Alpha0:
                ChangeOwner(Owner.None);
                break;
            case KeyCode.Alpha1:
                ChangeOwner(Owner.PlayerOne);
                break;
            case KeyCode.Alpha2:
                ChangeOwner(Owner.PlayerTwo);
                break;           
        }
    }

    private static void ChangeOwner(Owner ownership)
    {
        PlayerOwnership[] playerOwnerships = tile.GetComponentsInChildren<PlayerOwnership>();
        foreach(var p in playerOwnerships)
        {
            p.InstantOwnerChange(ownership);
        }
    }

    private static void UndoPlacement()
    {
        RemoveTile();
        tilePos.Mode = TilePosition.PositionMode.HOVER;
    }

    private static void DoPlacement()
    {
        if (!hexMap.TryGetTile(tile.Coordinates.Coord, out Tile _))
        {
            tilePos.AttachToGrid();
            tile.GetComponentInChildren<MeshCollider>().enabled = true; // hot fix
            hexMap.AddTile(tile);

            if (Application.isPlaying)
                tile.Connect();
        }

        Unset();
    }

    private static void RemoveTile()
    {
        hexMap.RemoveTile(tile.Coordinates.Coord);
        if (Application.isPlaying)
            tile.Disconnect();        
    }

    private static void DestroyTile()
    {
        RemoveTile();
        GameObject.DestroyImmediate(tile.gameObject);
        Unset();
    }
}
