using System;
using UnityEngine.InputSystem;

namespace Greenyas.Input
{
    public abstract class InputCallback
    {
        protected static readonly Action EmptyCallback = delegate { };
    } 
}
