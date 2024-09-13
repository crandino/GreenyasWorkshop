using Hexalinks.Tile;
using UnityEngine;

public class TileSelectionTurnStep : TurnStep
{
    [SerializeField]
    private Hand hand;   

    public override void Begin()
    {
        hand.ActivateSelection(OnTileSelection);
    }

    private void OnTileSelection(Tile tile)
    {
        tile.Initialize();
        tile.PickUp();

        Next();
    }

    protected override void End()
    {
        hand.DeactivateSelection();
    }
}
