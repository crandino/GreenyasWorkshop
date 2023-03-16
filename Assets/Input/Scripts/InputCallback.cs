using System;
using UnityEngine.InputSystem;

namespace Greenyass.Input
{
    public abstract class InputCallback
    {
        protected static readonly Action EmptyCallback = delegate { };
    } 
}
