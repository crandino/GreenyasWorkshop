using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CollectionExtensions
{
    private readonly static System.Random random = new System.Random();

    public static void Shuffle<T>(this IEnumerable<T> collection)
    {
        collection = collection.OrderBy(item => random.Next());
    }

    public static void Shuffle<T>(this T[] list)
    {
        int n = list.Length;
        while (n > 1)
        {
            int i = random.Next(n--); // 0 ≤ i < n
            T t = list[n];
            list[n] = list[i];
            list[i] = t;
        }
    }
}
