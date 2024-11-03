using HexaLinks.Tile;
using UnityEngine;
using HexaLinks.UI.PlayerHand;

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

        Game.Instance.GetSystem<TilePlacement>().Start(tile);

        Next();
    }

    protected override void End()
    {
        hand.DeactivateSelection();
    }
}
