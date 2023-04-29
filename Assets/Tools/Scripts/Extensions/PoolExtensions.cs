using System;
using System.Collections.Generic;
using UnityEngine.Pool;

public static class PoolExtensions
{
    private static void Release<T>(this ObjectPool<T> pool, IEnumerable<T> collectionToRelease) where T : class
    {
        Release(pool, collectionToRelease, t => true);
    }

    public static void Release<T>(this ObjectPool<T> pool, IEnumerable<T> collectionToRelease, Func<T, bool> predicate ) where T : class
    {
        IEnumerator<T> enumerator = collectionToRelease.GetEnumerator();

        while (enumerator.MoveNext())
        {
            if (predicate(enumerator.Current))
                pool.Release(enumerator.Current);
        }
    }
}
