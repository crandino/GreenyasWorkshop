using System;
using UnityEngine.InputSystem;

namespace Greenyas.Input
{
    public class InputButtonCallback : InputCallback
    {
        private Action onButtonPressed = EmptyCallback;
        private Action onButtonReleased = EmptyCallback;

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
