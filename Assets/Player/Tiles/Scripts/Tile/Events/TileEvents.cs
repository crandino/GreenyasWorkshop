using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

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
        public static EventType<EmptyArgs> OnPropagationStepEnded = new();

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

            public void Clear()
            {
                unlockCallbacks.Callbacks = null;
                lockCallbacks.ForEach(l => l.Callbacks = null);
            }
        }

        public static void Clear()
        {
            OnTurnEnded.Clear();
            OnSegmentConnected.Clear();
            OnPropagationStep.Clear();
            OnPropagationEnded.Clear();
            OnPropagationStepEnded.Clear();

            OnSegmentPropagated.Clear();
        }
    }
}

