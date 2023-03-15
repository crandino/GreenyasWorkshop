using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Greenyass.Input
{
    public static class InputManager
    {
        private static PlayerActions actions = null;

        public static InputButtonCallback OnSelection;

        public static void Init()
        {
            actions = new PlayerActions();
            actions.Enable();

            OnSelection = new InputButtonCallback(actions.InGame.Selection);
        }
    }    
}

