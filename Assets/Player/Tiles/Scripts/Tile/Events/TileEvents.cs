using System;

namespace HexaLinks.Tile.Events
{
    public static class TileEvents
    {
        public static EventType OnSegmentConnected = new();
        public static EventTypeArgs<OnSegmentPropagatedArgs> OnSegmentPropagated = new();

        public class EventType
        {
            public event Action Callbacks;

            public void Call()
            {
                Callbacks?.Invoke();
            }
        }

        public class EventTypeArgs<T>
        {
            public event Action<T> Callbacks;

            public void Call(T args)
            {
                Callbacks?.Invoke(args);
            }
        }
    }
}

