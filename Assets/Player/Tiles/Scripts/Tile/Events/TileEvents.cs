using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace HexaLinks.Tile.Events
{
    public static class TileEvents
    {
        public readonly struct EmptyArgs { }

        public readonly static EmptyArgs Empty = new EmptyArgs();

        public static EventType<EmptyArgs> OnTurnEnded = new();
        public static EventType<EmptyArgs> OnSegmentConnected = new(); 
        public static EventType<EmptyArgs> OnPropagationStep = new();
        public static EventType<EmptyArgs> OnPropagationEnded = new();

        public static EventType<OnSegmentPropagatedArgs> OnSegmentPropagated = new();  
        
        public class EventType<T> where T : struct
        {
            private class EventData
            {
                public UnityEngine.Object Subscriber { private set; get; }
                public Action<T?> Callbacks;

                public EventData(UnityEngine.Object subscriber = null)
                {
                    Subscriber = subscriber;
                    Callbacks = null;
                }
            }

            private EventData unlockCallbacks = new EventData();
            private readonly List<EventData> lockCallbacks = new List<EventData>();

            public void RegisterCallback(Action<T?> callback) => unlockCallbacks.Callbacks += callback;
            public void UnregisterCallback(Action<T?> callback) => unlockCallbacks.Callbacks -= callback;
            public void RegisterCallback(UnityEngine.Object subscriber, Action<T?> callback)
            {
                int index = lockCallbacks.FindIndex(data => data.Subscriber == subscriber);

                if (index == -1)
                {
                    lockCallbacks.Add(new EventData(subscriber));
                    index = lockCallbacks.Count - 1;
                }

                lockCallbacks[index].Callbacks += callback;
            }

            public void UnregisterCallbacks(UnityEngine.Object subscriber)
            {
                int index = lockCallbacks.FindIndex(data => data.Subscriber == subscriber);
                Assert.IsTrue(index != -1, $"There's no registered callback for subscriber {subscriber}");
                lockCallbacks[index].Callbacks = null;
            }

            public void Call(T? args)
            {
                unlockCallbacks.Callbacks?.Invoke(args);
            }

            public void Call(UnityEngine.Object subscriber, T? args)
            {
                int index = lockCallbacks.FindIndex(data => data.Subscriber == subscriber);
                Assert.IsTrue(index != -1, $"There's no registered callback for subscriber {subscriber}");
                lockCallbacks[index].Callbacks?.Invoke(args);
            }

            //public void UnregisterPermamentCallback(Action<T?> callback) => Callbacks -= callback;

        }

        //public class EventTypeArgs<T> where T : struct
        //{
        //    private event Action<T?> Callbacks, VolatileCallbacks;

        //    public void RegisterPermamentCallback(Action<T?> callback) => Callbacks += callback;
        //    public void UnregisterPermamentCallback(Action<T?> callback) => Callbacks -= callback;

        //    public void RegisterVolatileCallback(Action<T?> callback) => VolatileCallbacks += callback;

        //    public void Clean(bool includePermanent = false)
        //    {
        //        UnityEngine.Assertions.Assert.IsTrue(VolatileCallbacks.GetInvocationList().Length > 0, "There's no volatile callback to clean. Is that intended?");
                
        //        VolatileCallbacks = null;
        //        if (includePermanent)
        //            Callbacks = null;
        //    }

        //    public void Call(T? args = EmptyArgs)
        //    {
        //        Callbacks?.Invoke(args);
        //        VolatileCallbacks?.Invoke(args);
        //    }
        //}

    }
}

