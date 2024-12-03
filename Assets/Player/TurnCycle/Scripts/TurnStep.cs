using System;
using UnityEngine;

public abstract class TurnStep
{
    private readonly Action endTurnStep = null;

    protected TurnStep(Action endTurnStep)
    {
        this.endTurnStep = endTurnStep;
    }

    public abstract void Begin(TurnManager.PlayerContext context);
    
    protected virtual void End()
    {
        endTurnStep();
    }
}


