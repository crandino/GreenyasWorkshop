using HexaLinks.Ownership;
using System;

public static class TileEvents
{
    public static EventType OnSegmentConnected = new(true);
    public static EventType OnSegmentPropagated = new();

    public static PlayerOwnership.Ownership Owner { get; set; } = PlayerOwnership.Ownership.None;

    public class EventType
    {
        private event Action<PlayerOwnership.Ownership> Callbacks;
        private readonly Func<PlayerOwnership.Ownership, bool> predicate = (owner) => true;

        public EventType(bool exclusiveOwnershipCallback = false)
        {
            if (exclusiveOwnershipCallback)
                predicate = (owner) => owner == Owner;
        }

        public bool Enabled { get; set; } = true;

        public void Register(Action<PlayerOwnership.Ownership> callback)
        {
            Callbacks += callback;
        }

        public void Unregister(Action<PlayerOwnership.Ownership> callback)
        {
            Callbacks -= callback;
        }

        public bool EmptyCallback { get { return Callbacks.GetInvocationList().Length == 0; } }

        public void Call()
        {
            Call(Owner);
        }

        public void Call(PlayerOwnership.Ownership owner)
        {
            if (Enabled && predicate(owner)) 
                Callbacks.Invoke(owner);
        }
    }
}


