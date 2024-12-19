using System;

namespace HexaLinks.Turn
{
    using UI.PlayerHand;

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

            tilePlacement.AddEvents(End, hand.Activate);
        }

        protected override void End()
        {
            hand.Deactivate();
            tilePlacement.RemoveEvents(End, hand.Activate);

            base.End();
        }
    }

}