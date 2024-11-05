using UnityEngine;

public abstract class TurnStep
{
    protected void Next()
    {
        TurnManager.Steps.NextStep();        
    }

    public abstract void Begin(TurnManager.PlayerContext context);
    public virtual void End() { }
}


