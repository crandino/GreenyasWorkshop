using System;

namespace HexaLinks.Turn
{
    public abstract class TurnStep
    {
        protected readonly TurnManager.TurnSteps turnSteps = null;

        protected TurnStep(TurnManager.TurnSteps turnSteps )
        {
            this.turnSteps = turnSteps;
        }

        public abstract void Begin();
        public virtual void SafeEnd() { }
    } 
}


