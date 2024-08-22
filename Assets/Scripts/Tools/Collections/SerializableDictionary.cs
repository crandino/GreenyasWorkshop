using System;
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

    public SerializableDictionary(IEqualityComparer<TKey> keyComparer) : base(keyComparer) { }

    public void OnBeforeSerialize()
    {
        keys = Keys.ToArray();
        values = Values.ToArray();
    }

    public void OnAfterDeserialize()
    {
        base.Clear();
        for (int i = 0; i < keys.Length; i++)
            Add(keys[i], values[i]);
    }

    new public void Clear()
    {
        base.Clear();
        keys = new TKey[0];
        values = new TValue[0];
    }
}
