using HexaLinks.Ownership;
using NUnit.Framework;
using System;
using System.Collections.Generic;

public static class TileEvents
{
    public static EventType onSegmentConnected = new EventType();

    private readonly static Dictionary<PlayerOwnership.Ownership, List<EventType>> callbacks = new();

    private static void RegisterEvent(PlayerOwnership.Ownership owner, EventType eventType)
    {
        if (!callbacks.ContainsKey(owner))
            callbacks.Add(owner, new List<EventType>());

        List<EventType> listOfEvents = callbacks[owner];
        if (listOfEvents.Find(e => e == eventType) == null)
            listOfEvents.Add(eventType);
    }

    private static void UnregisterEvent(PlayerOwnership.Ownership owner, EventType eventType)
    {
        if (!callbacks.ContainsKey(owner))
            callbacks.Add(owner, new List<EventType>());

        List<EventType> listOfEvents = callbacks[owner];
        Assert.IsTrue(listOfEvents.Contains(eventType), $"Event {eventType} has not been previously registered");
        if ( eventType.EmptyCallback)
            listOfEvents.Remove(eventType);
    }

    public static void EnableCallbacks(params PlayerOwnership.Ownership[] owners)
    {
        foreach(var owner in owners)
        {
            if(callbacks.TryGetValue(owner, out List<EventType> listOfEvents))
            {
                foreach(var entry in listOfEvents)
                    entry.Enabled = true;
            }
        }
    }

    public static void DisableCallbacks(params PlayerOwnership.Ownership[] owners)
    {
        foreach (var owner in owners)
        {
            if (callbacks.TryGetValue(owner, out List<EventType> listOfEvents))
            {
                foreach (var entry in listOfEvents)
                    entry.Enabled = false;
            }
        }
    }

    public class EventType
    {
        public bool Enabled { get; set; } = true;

        public void Register(PlayerOwnership.Ownership owner, Action callback)
        {
            Callback += callback;            
            RegisterEvent(owner, this);
        }

        public void Unregister(PlayerOwnership.Ownership owner, Action callback)
        {
            Callback -= callback;
            UnregisterEvent(owner, this);
        }

        public bool EmptyCallback { get { return Callback == null || Callback.GetInvocationList().Length == 0; } }

        private event Action Callback;

        public void Invoke()
        {
            if(Enabled)
                Callback?.Invoke();
        }
    }
}


