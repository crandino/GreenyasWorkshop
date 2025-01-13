using Greenyas.Input;
using HexaLinks.Tile;
using UnityEngine;
using EventType = HexaLinks.Events.EventType;

public class TilePlacement : Game.IGameSystem
{
    private InputManager input = null;
    private Tile currentSelectedTile = null;

    public void InitSystem()
    {
        input = Game.Instance.GetSystem<InputManager>();
    }

    public void TerminateSystem()
    {
        FinishPlacement();
        Events.Clear();
    }

    public void Start(Tile tile)
    {
        currentSelectedTile = tile;

        tile.Initialize();
        tile.PickUp();

        input.TilePlacement.OnButtonPressed += TryPlacement;
        input.TilePlacementCancellation.OnButtonPressed += CancelPlacement;
    }

    // Include that as a DEBUG feature
    private void PickUpTile()
    {
        if (TileRaycast.CursorRaycastToTile(out currentSelectedTile))
        {
            currentSelectedTile.PickUp();
        }       
    }

    private void TryPlacement()
    {
        if (!TileRaycast.CursorRaycastToTile() && currentSelectedTile.TryRelease())
        {
            Events.OnSuccessPlacement.Call();
            FinishPlacement();
        }
    }

    private void CancelPlacement()
    {
        currentSelectedTile.Terminate();
        GameObject.Destroy(currentSelectedTile.gameObject);

        Events.OnFailurePlacement.Call();
        FinishPlacement();
    }

    private void FinishPlacement()
    {
        currentSelectedTile = null;
        Events.OnFinishPlacement.Call();

        input.TilePlacement.OnButtonPressed -= TryPlacement;
        input.TilePlacementCancellation.OnButtonPressed -= CancelPlacement;
    }

    public static class Events
    {
        public readonly static EventType OnSuccessPlacement = new();
        public readonly static EventType OnFailurePlacement = new();
        public readonly static EventType OnFinishPlacement = new();

        public static void Clear()
        {
            OnSuccessPlacement.Clear();
            OnFailurePlacement.Clear();
            OnFinishPlacement.Clear();
        }
    }
}
