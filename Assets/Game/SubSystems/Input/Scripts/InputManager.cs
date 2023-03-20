namespace Greenyass.Input
{
    public class InputManager : Game.SubSystem
    {
        private PlayerActions actions = null;

        public InputDeltaCallback OnAxis;
        public InputButtonCallback OnSelect;

        protected override bool TryInitSystem()
        {
            actions = new PlayerActions();
            actions.Enable();

            OnSelect = new InputButtonCallback(actions.InGame.Selection);
            OnAxis = new InputDeltaCallback(actions.InGame.RotatePiece);

            return true;
        }
    }    
}

