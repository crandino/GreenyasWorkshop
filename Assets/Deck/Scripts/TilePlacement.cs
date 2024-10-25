using Greenyas.Input;
using HexaLinks.Tile;
using System;
using UnityEngine;

public class TilePlacement : Game.SubSystem
{
    private Tile currentSelectedTile = null;
    private InputManager input = null;

    public event Action OnSuccessPlacement;
    public event Action OnFailurePlacement;

    protected override bool TryInitSystem()
    {
        input = Game.Instance.GetSystem<InputManager>();
        return true;
    }

    private void PickUpTile()
    {
        // Include that as a DEBUG feature
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
        GameObject.Destroy(currentSelectedTile.gameObject);

        OnFailurePlacement();
        Finish();
    }

    public void Start(Tile tile)
    {
        currentSelectedTile = tile;

        input.TilePlacement.OnButtonPressed += TryReleaseTile;
        input.TilePlacementCancellation.OnButtonPressed += CancelTile;
    }  

    private void Finish()
    {
        currentSelectedTile = null;

        input.TilePlacement.OnButtonPressed -= TryReleaseTile;
        input.TilePlacementCancellation.OnButtonPressed -= CancelTile;
    }
}
