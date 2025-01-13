namespace HexaLinks.Turn
{
    public class DeckDrawingTurnStep : TurnStep
    {
        public DeckDrawingTurnStep(TurnManager.TurnSteps turnSteps) : base(turnSteps)
        { }

        public override void Begin()
        {
            turnSteps.CurrentContext.hand.Draw();
            turnSteps.NextStep();
        }
    } 
}
