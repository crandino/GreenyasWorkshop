namespace Greenyass.Input
{
    public static class InputManager
    {
        private static PlayerActions actions = null;

        public static InputDeltaCallback OnAxis;

        public static void Init()
        {
            actions = new PlayerActions();
            actions.Enable();

            OnAxis = new InputDeltaCallback(actions.InGame.RotatePiece);
        }
    }    
}

