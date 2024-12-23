using System;
using System.Linq;

namespace HexaLinks.Events
{
    public class EventType
    {
        private Action callbacks = delegate { };

        public void Register(Action callback) => callbacks += callback;
        public void Unregister(Action callback) => callbacks -= callback;

        public void Unregister(object target)
        {
            Delegate[] delegates = callbacks.GetInvocationList().Where(l => l.Target != target).ToArray();
            UnityEngine.Debug.Log($"Before: {delegates.Length}");
            foreach (Delegate del in delegates)
            {
                callbacks -= (Action)del;
            }
            UnityEngine.Debug.Log($"After: {callbacks.GetInvocationList().Where(l => l.Target != target).ToArray().Length}");
        }

        public void Call(object onlyTarget)
        {
            Delegate[] delegates = callbacks.GetInvocationList().Where(l => l.Target == onlyTarget).ToArray();
            foreach (Delegate del in delegates)
            {
                del.DynamicInvoke();
            }
        }

        public void Call()
        {
            callbacks.DynamicInvoke();
        }
    }

    public class EventTypeArg<T> where T : struct
    {
        private Action<T> callbacks = delegate { };

        public void Register(Action<T> callback) => callbacks += callback;
        public void Unregister(Action<T> callback) => callbacks -= callback;

        //public void Unregister(object target)
        //{
        //    Delegate[] delegates = callbacks.GetInvocationList().Where(l => l.Target == target).ToArray();
        //    foreach (Delegate del in delegates)
        //    {
        //        callbacks.;
        //    }

        public void Call(object target, T args)
        {
            Delegate[] delegates = callbacks.GetInvocationList().Where(l => l.Target == target).ToArray();
            foreach (Delegate del in delegates)
            {
                del.DynamicInvoke(args);
            }
        }

        public void Call(T args)
        {
            callbacks.DynamicInvoke(args);
        }
    }
}

