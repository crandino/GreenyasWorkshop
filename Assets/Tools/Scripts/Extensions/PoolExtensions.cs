using System.Collections.Generic;
using UnityEngine.Pool;

public static class PoolExtensions
{
    //public static void Release<T>(this ObjectPool<T> pool, IEnumerable<T> collectionToRelease) where T : class
    //{
    //    Release(pool, collectionToRelease, t => true);
    //}

    public static void Release<T>(this ObjectPool<T> pool, IEnumerable<T> collectionToRelease ) where T : class
    {
        IEnumerator<T> enumerator = collectionToRelease.GetEnumerator();

        while (enumerator.MoveNext())
            pool.Release(enumerator.Current);
    }
}
