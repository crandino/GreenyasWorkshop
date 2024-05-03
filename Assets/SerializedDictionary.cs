using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public sealed class SerializedDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [System.Serializable]
    private class SerializableKeyValuePair
    {
        [SerializeField]
        private TKey key;

        [SerializeField]
        private TValue value;

        public SerializableKeyValuePair()
        {
            key = default;
            value = default;
        }

        public SerializableKeyValuePair(TKey key, TValue value)
        {
            this.key = key;
            this.value = value;
        }

        public SerializableKeyValuePair(KeyValuePair<TKey, TValue> pair)
        {
            key = pair.Key;
            value = pair.Value;
        }

        public TKey Key => key;

        public TValue Value => value;

        //public override string ToString()
        //{
        //    return ((KeyValuePair<TKey, TValue>)this).ToString();
        //}

        public static implicit operator KeyValuePair<TKey, TValue>(SerializableKeyValuePair serializedPair)
        {
            return new KeyValuePair<TKey, TValue>(serializedPair.key, serializedPair.value);
        }
    }

    [SerializeField] 
    private SerializableKeyValuePair[] pairs;

    public void OnAfterDeserialize()
    {
        Clear();

        for (int i = 0; i < pairs.Length; i++)
        {
            this[pairs[i].Key] = pairs[i].Value;
        }
    }

    public void OnBeforeSerialize()
    {
        pairs = new SerializableKeyValuePair[Count];

        Enumerator enumerator = GetEnumerator();
        int index = 0;

        while (enumerator.MoveNext())
            pairs[index++] = new SerializableKeyValuePair(enumerator.Current);
    }
}
