using HexaLinks.UI.PlayerHand;

public class TileSelectionTurnStep : TurnStep
{
    private TilePlacement tilePlacement;
    private Hand hand;

    public override void Begin(TurnManager.PlayerContext context)
    {
        hand = context.hand;
        hand.Activate();

        tilePlacement = Game.Instance.GetSystem<TilePlacement>();
        tilePlacement.OnSuccessPlacement += Next;
        tilePlacement.OnFailurePlacement += hand.Activate;
    }   

    public override void End()
    {
        tilePlacement.OnSuccessPlacement -= Next;
        tilePlacement.OnFailurePlacement -= hand.Activate;

        hand.Deactivate();
    }
}
