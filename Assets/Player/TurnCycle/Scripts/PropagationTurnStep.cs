using HexaLinks.Path.Finder;
using HexaLinks.Propagation;
using HexaLinks.Tile.Events;
using System;

namespace HexaLinks.Turn
{
    public class PropagationTurnStep : TurnStep
    {
        //private readonly PropagationManager propagation;

        public PropagationTurnStep(Action endTurnStep) : base(endTurnStep)
        {
            //propagation = Game.Instance.GetSystem<PropagationManager>();
        }

        public override void Begin(TurnManager.PlayerContext context)
        {
            if(PathIterator.ArePendingSearches)
            {
                TileEvents.OnPropagationEnded.RegisterCallback(EndStep);
                PathIterator.TriggerSearch(null);
            }
            else
                End();
        }

        protected override void End()
        {
            TileEvents.OnPropagationEnded.UnregisterCallback(EndStep);
            base.End();
        }

        private void EndStep(TileEvents.EmptyArgs? args) => End();
    }

}