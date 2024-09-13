using Greenyas.Input;
using Hexalinks.Tile;
using System;

public class TilePlacement : Game.SubSystem
{
    private Tile currentSelectedTile = null;
    private InputManager input = null;

    public event Action OnSuccessPlacement;
    public event Action OnFailurePlacement;

    protected override bool TryInitSystem()
    {
        input = Game.Instance.GetSystem<InputManager>();
        //input.OnSelect.OnButtonPressed += PickUpTile;
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
        if (!TileRaycast.CursorRaycastToTile())
        {
            OnSuccessPlacement();

            currentSelectedTile.Release();
            currentSelectedTile = null;
        }
    }

    private void CancelTile()
    {
        OnFailurePlacement();
        currentSelectedTile = null;
    }

    public void Start(Tile tile)
    {
        currentSelectedTile = tile;

        input.TilePlacement.OnButtonPressed += TryReleaseTile;
        input.TilePlacementCancellation.OnButtonPressed += CancelTile;
    }  

    public void Finish()
    {
        input.TilePlacement.OnButtonPressed -= TryReleaseTile;
        input.TilePlacementCancellation.OnButtonPressed -= CancelTile;

        currentSelectedTile = null;
    }
}
