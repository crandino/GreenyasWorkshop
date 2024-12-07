using HexaLinks.UI.PlayerHand;
using System;

public class TileSelectionTurnStep : TurnStep
{
    private readonly TilePlacement tilePlacement;
    private Hand hand;

    public TileSelectionTurnStep(Action endTurnStep) : base(endTurnStep)
    {
        tilePlacement = Game.Instance.GetSystem<TilePlacement>();
    }

    public override void Begin(TurnManager.PlayerContext context)
    {
        hand = context.hand;
        hand.Activate();

        tilePlacement.OnSuccessPlacement += End;
        tilePlacement.OnFailurePlacement += hand.Activate;
    }   

    protected override void End()
    {
        hand.Deactivate();

        tilePlacement.OnSuccessPlacement -= End;
        tilePlacement.OnFailurePlacement -= hand.Activate;

        base.End();
    }
}
