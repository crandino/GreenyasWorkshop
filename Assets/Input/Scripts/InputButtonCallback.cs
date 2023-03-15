using System;
using UnityEngine.InputSystem;

namespace Greenyass.Input
{
    public class InputButtonCallback : InputCallback
    {
        private Action onButtonPressed = Empty;
        private Action onButtonReleased = Empty;

        public InputButtonCallback(InputAction action)
        {
            action.started += ButtonPressed;
            action.canceled += ButtonReleased;
        }

        public event Action OnButtonPressed
        {
            add => onButtonPressed += value;
            remove => onButtonPressed -= value;
        }

        public event Action OnButtonReleased
        {
            add => onButtonReleased += value;
            remove => onButtonReleased -= value;
        }

        private void ButtonPressed(InputAction.CallbackContext context) => onButtonPressed();
        private void ButtonReleased(InputAction.CallbackContext context) => onButtonReleased();
    } 
}
