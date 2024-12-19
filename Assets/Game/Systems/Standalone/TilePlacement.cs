using Greenyas.Input;
using HexaLinks.Tile;
using System;
using UnityEngine;

public class TilePlacement : Game.IGameSystem
{
    private Tile currentSelectedTile = null;
    private InputManager input = null;

    private event Action OnSuccessPlacement;
    private event Action OnFailurePlacement;

    public void InitSystem()
    {
        input = Game.Instance.GetSystem<InputManager>();
    }

    public void Start(Tile tile)
    {
        currentSelectedTile = tile;

        tile.Initialize();
        tile.PickUp();

        input.TilePlacement.OnButtonPressed += TryPlacement;
        input.TilePlacementCancellation.OnButtonPressed += CancelPlacement;
    }

    public void AddEvents(Action onSuccess, Action onFailure = null)
    {
        OnSuccessPlacement += onSuccess;
        if(onFailure != null)
            OnFailurePlacement += onFailure;
    }

    public void RemoveEvents(Action onSuccess, Action onFailure = null)
    {
        OnSuccessPlacement -= onSuccess;
        if (onFailure != null)
            OnFailurePlacement -= onFailure;
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
            OnSuccessPlacement();
            FinishPlacement();
        }
    }

    private void CancelPlacement()
    {
        currentSelectedTile.Terminate();
        GameObject.Destroy(currentSelectedTile.gameObject);

        OnFailurePlacement();
        FinishPlacement();
    }

    private void FinishPlacement()
    {
        currentSelectedTile = null;

        input.TilePlacement.OnButtonPressed -= TryPlacement;
        input.TilePlacementCancellation.OnButtonPressed -= CancelPlacement;
    }
}
