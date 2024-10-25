using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHashable<in Key, HashValue>
{
    public HashValue Hash { get; }

    public HashValue HashFunction(Key key);
}
