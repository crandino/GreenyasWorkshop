using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver, IDictionary
{
    [SerializeField]
    private TKey[] keys;

    [SerializeField]
    private TValue[] values;

    [SerializeField]
    private List<InvalidKey> invalidKeyValuePairs = new List<InvalidKey>();

    [System.Serializable]
    private struct InvalidKey
    {
        public KeyValuePair<TKey, TValue> keyValuePair;
        public int keyArrayIndex;

        public InvalidKey(TKey key, TValue value, int index)
        {
            keyValuePair = new KeyValuePair<TKey, TValue>(key, value);
            keyArrayIndex = index;
        }
    }

    public SerializableDictionary() : base() { }

    public SerializableDictionary(IEqualityComparer<TKey> keyComparer) : base(keyComparer) { }

    public void OnBeforeSerialize()
    {
        keys = Enumerable.Concat(Keys, invalidKeyValuePairs.Select(x => x.keyValuePair.Key)).ToArray();
        values = Enumerable.Concat(Values, invalidKeyValuePairs.Select(x => x.keyValuePair.Value)).ToArray();
    }

    public void OnAfterDeserialize()
    {
        base.Clear();
        invalidKeyValuePairs.Clear();

        for (int i = 0; i < keys.Length; i++)
        {
            if (IsKeyInvalid(keys[i]))
                invalidKeyValuePairs.Add(new InvalidKey(keys[i], values[i], i));
            else
                Add(keys[i], values[i]);
        }
    }

    private bool IsKeyInvalid(TKey keyToCheck)
    {
        return keyToCheck == null || keys.Count(k => keyToCheck.Equals(k)) > 1;
    }

    new public void Clear()
    {
        base.Clear();
        invalidKeyValuePairs.Clear();

        keys = new TKey[0];
        values = new TValue[0];
    }
}
