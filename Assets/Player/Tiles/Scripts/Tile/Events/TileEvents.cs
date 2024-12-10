using System;

namespace HexaLinks.Tile.Events
{
    public static class TileEvents
    {
        public readonly struct EmptyArgs { }

        public static EventTypeArgs<EmptyArgs> OnTurnEnded = new();
        public static EventTypeArgs<EmptyArgs> OnSegmentConnected = new(); 
        public static EventTypeArgs<EmptyArgs> OnPropagationStep = new();

        public static EventTypeArgs<OnSegmentPropagatedArgs> OnSegmentPropagated = new();       

        public class EventTypeArgs<T> where T : struct
        {
            private event Action<T?> Callbacks, VolatileCallbacks;

            public void RegisterPermamentCallback(Action<T?> callback) => Callbacks += callback;
            public void UnregisterPermamentCallback(Action<T?> callback) => Callbacks -= callback;

            public void RegisterVolatileCallback(Action<T?> callback) => VolatileCallbacks += callback;
            public void UnregisterVolatileCallback(Action<T?> callback) => VolatileCallbacks -= callback;

            public void Clean(bool includePermanent = false)
            {
                UnityEngine.Assertions.Assert.IsTrue(VolatileCallbacks.GetInvocationList().Length > 0, "There's no volatile callback to clean. Is that intended?");
                
                VolatileCallbacks = null;
                if (includePermanent)
                    Callbacks = null;
            }

            public void Call(T? args)
            {
                Callbacks?.Invoke(args);
                VolatileCallbacks?.Invoke(args);
            }
        }

    }
}

