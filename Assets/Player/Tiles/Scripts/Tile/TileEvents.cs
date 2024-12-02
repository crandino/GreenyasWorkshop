using HexaLinks.Ownership;
using System;

public static class TileEvents
{
    public static EventType OnSegmentConnected = new();

    public static PlayerOwnership.Ownership Owner { get; set; } = PlayerOwnership.Ownership.None;

    public class EventType
    {
        private event Action<PlayerOwnership.Ownership> Callbacks;       

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
            if(Enabled)
                Callbacks.Invoke(Owner);
        }
    }
}


