using System;

namespace HexaLinks.Turn
{
    using UI.PlayerHand;

    public class TileSelectionTurnStep : TurnStep
    {
        private Hand hand;

        public TileSelectionTurnStep(Action endTurnStep) : base(endTurnStep)
        { }

        public override void Begin(TurnManager.PlayerContext context)
        {
            hand = context.hand;
            hand.Activate();

            TilePlacement.Events.OnSuccessPlacement.Register(End);
            TilePlacement.Events.OnFailurePlacement.Register(hand.Activate);
        }

        protected override void End()
        {
            hand.Deactivate();

            TilePlacement.Events.OnSuccessPlacement.Unregister(End);
            TilePlacement.Events.OnFailurePlacement.Unregister(hand.Activate);

            base.End();
        }
    }

}