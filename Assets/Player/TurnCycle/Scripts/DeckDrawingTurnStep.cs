using System;

public class DeckDrawingTurnStep : TurnStep
{
    public DeckDrawingTurnStep(Action endTurnStep) : base(endTurnStep)
    { }

    public override void Begin(TurnManager.PlayerContext context)
    {
        context.hand.Draw();
        base.End();
    }
}
