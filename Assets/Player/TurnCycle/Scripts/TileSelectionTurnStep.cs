namespace HexaLinks.Turn
{
    public class TileSelectionTurnStep : TurnStep
    {
        private bool successPlacement = false;

        public TileSelectionTurnStep(TurnManager.TurnSteps turnSteps) : base(turnSteps)
        { }

        public override void Begin()
        {
            successPlacement = false;

            TilePlacement.Events.OnSuccessPlacement.Register(OnTileSelectionSuccess);
            TilePlacement.Events.OnFinishPlacement.Register(NextStep);
        }

        private void OnTileSelectionSuccess()
        {
            successPlacement = true;
        }

        private void NextStep()
        {
            if (successPlacement)
            {
                SafeEnd();
                turnSteps.NextStep();
            }
        }

        public override void SafeEnd()
        {
            TilePlacement.Events.OnSuccessPlacement.Unregister(OnTileSelectionSuccess);
            TilePlacement.Events.OnFinishPlacement.Unregister(NextStep);
        }
    }
}