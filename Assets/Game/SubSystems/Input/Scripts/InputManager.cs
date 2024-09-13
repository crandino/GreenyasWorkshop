namespace Greenyas.Input
{
    public class InputManager : Game.SubSystem
    {
        private PlayerActions actions = null;

        public InputDeltaCallback OnAxis;

        public InputButtonCallback TilePlacement;
        public InputButtonCallback TilePlacementCancellation;

        protected override bool TryInitSystem()
        {
            actions = new PlayerActions();
            actions.Enable();

            TilePlacement = new InputButtonCallback(actions.InGame.TilePlacement);
            TilePlacementCancellation = new InputButtonCallback(actions.InGame.CancelTilePlacement);
            OnAxis = new InputDeltaCallback(actions.InGame.RotatePiece);

            return true;
        }
    }    
}

