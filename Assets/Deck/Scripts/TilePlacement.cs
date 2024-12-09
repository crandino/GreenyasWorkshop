using Greenyas.Input;
using HexaLinks.Tile;
using System;
using UnityEngine;

public class TilePlacement : Game.IGameSystem
{
    private Tile currentSelectedTile = null;
    private InputManager input = null;

    public event Action OnSuccessPlacement;
    public event Action OnFailurePlacement;

    public void InitSystem()
    {
        input = Game.Instance.GetSystem<InputManager>();
    }

    public void Start(Tile tile)
    {
        currentSelectedTile = tile;

        tile.Initialize();
        tile.PickUp();

        input.TilePlacement.OnButtonPressed += TryReleaseTile;
        input.TilePlacementCancellation.OnButtonPressed += CancelTile;
    }

    // Include that as a DEBUG feature
    private void PickUpTile()
    {
        if (TileRaycast.CursorRaycastToTile(out currentSelectedTile))
        {
            currentSelectedTile.PickUp();
        }       
    }

    private void TryReleaseTile()
    {
        if (!TileRaycast.CursorRaycastToTile() && currentSelectedTile.TryRelease())
        {
            OnSuccessPlacement();
            Finish();
        }
    }

    private void CancelTile()
    {
        currentSelectedTile.Terminate();
        GameObject.Destroy(currentSelectedTile.gameObject);

        OnFailurePlacement();
        Finish();
    }

   

    private void Finish()
    {
        currentSelectedTile = null;

        input.TilePlacement.OnButtonPressed -= TryReleaseTile;
        input.TilePlacementCancellation.OnButtonPressed -= CancelTile;
    }
}
