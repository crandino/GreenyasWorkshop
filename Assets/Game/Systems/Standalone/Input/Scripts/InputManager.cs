namespace Greenyas.Input
{
    public class InputManager : Game.IGameSystem
    {
        private PlayerActions actions = null;

        public InputDeltaCallback OnAxis;

        public InputButtonCallback TilePlacement;
        public InputButtonCallback TilePlacementCancellation;

        public void InitSystem()
        {
            actions = new PlayerActions();
            actions.Enable();

            TilePlacement = new InputButtonCallback(actions.InGame.TilePlacement);
            TilePlacementCancellation = new InputButtonCallback(actions.InGame.CancelTilePlacement);
            OnAxis = new InputDeltaCallback(actions.InGame.RotatePiece);
        }

        public void TerminateSystem() 
        {
        }
    }    
}

