using HexaLinks.Path.Finder;
using HexaLinks.Tile.Events;
using System;

namespace HexaLinks.Turn
{
    public class PropagationTurnStep : TurnStep
    {
        public PropagationTurnStep(Action endTurnStep) : base(endTurnStep)
        {
            TileEvents.OnPropagationEnded.RegisterCallback(EndStep);
        }

        public override void Begin(TurnManager.PlayerContext context)
        {
            PathIterator.TriggerSearch(null);
        }

        private void EndStep(TileEvents.EmptyArgs? args) => base.End();
    }

}