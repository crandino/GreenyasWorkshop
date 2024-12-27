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
        }

        protected override void End()
        {
            hand.Deactivate();

            TilePlacement.Events.OnSuccessPlacement.Unregister(End);

            base.End();
        }
    }

}