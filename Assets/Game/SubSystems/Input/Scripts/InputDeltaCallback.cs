using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Greenyass.Input
{
    public class InputDeltaCallback : InputCallback
    {
        private Action onPositiveDelta = EmptyCallback;
        private Action onNegativeDelta = EmptyCallback;
        private Action onAnyDelta = EmptyCallback;

        public InputDeltaCallback(InputAction action)
        {
            action.performed += OnDelta;
        }

        public event Action OnPositiveDelta
        {
            add => onPositiveDelta += value;
            remove => onPositiveDelta -= value;
        }

        public event Action OnNegativeDelta
        {
            add => onNegativeDelta += value;
            remove => onNegativeDelta -= value;
        }

        public event Action OnAnyDelta
        {
            add => onAnyDelta += value;
            remove => onAnyDelta -= value;
        }

        private void OnDelta(InputAction.CallbackContext context)
        {
            onAnyDelta();
            if(context.ReadValue<Vector2>().y > 0f)
                onPositiveDelta();
            else
                onNegativeDelta();
        }
    }
}
