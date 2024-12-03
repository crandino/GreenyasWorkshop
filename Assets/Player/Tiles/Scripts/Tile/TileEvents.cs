using HexaLinks.Ownership;
using System;
using static TileEvents.OnSegmentPropagatedEvent;

public static class TileEvents
{
    public static EventType OnSegmentConnected = new();
    public static OnSegmentPropagatedEvent OnSegmentPropagated = new();

    public class EventType
    {
        public event Action Callbacks;

        public void Call()
        {
            Callbacks?.Invoke();
        }
    }

    public abstract class EventTypeArgs<T>
    {
        public event Action<T> Callbacks;

        public void Call(T args)
        {
            Callbacks?.Invoke(args);
        }
    }

    public class OnSegmentPropagatedEvent : EventTypeArgs<OnSegmentPropagatedArgs>
    {
        public struct OnSegmentPropagatedArgs
        {
            public PlayerOwnership.Ownership lastSegmentOwner;
            public PlayerOwnership.Ownership newSegmentOwner;

            public readonly int GetScoreIncrement(PlayerOwnership.Ownership scoreOwner)
            {
                if (lastSegmentOwner != newSegmentOwner)
                {
                    if (scoreOwner == lastSegmentOwner)
                        return -1;

                    if (scoreOwner == newSegmentOwner)
                        return +1;
                }

                return 0;
            }
        }
    }
}


