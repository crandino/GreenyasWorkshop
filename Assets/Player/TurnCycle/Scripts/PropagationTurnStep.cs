using System;

namespace HexaLinks.Turn
{
    using Path.Finder;
    using Propagation;

    public class PropagationTurnStep : TurnStep
    {
        public PropagationTurnStep(Action endTurnStep) : base(endTurnStep)
        {
            PropagationManager.Events.OnPropagationEnded.Register(base.End);
        }

        public override void Begin(TurnManager.PlayerContext context)
        {
            PathIterator.TriggerSearch();
        }       
    }
}