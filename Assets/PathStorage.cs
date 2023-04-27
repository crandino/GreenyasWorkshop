using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class PathStorage
{
    private static WaitForSeconds interval = new WaitForSeconds(0.5f);
    private static List<TileSegment[]> sequences = new List<TileSegment[]>();

    static PathStorage()
    {
        sequences.Clear();
    }

    private static IEnumerator StartHighlight(TileSegment[] path)
    {
        for (int i = 0; i < path.Length; i++)
        {
            path[i].Highlight();
            yield return interval;
        }
    }

    public static void AddPath(TileSegment[] path)
    {
        sequences.Add(path);      
    }

    public static void ShowLastPath<T>(T mono) where T : MonoBehaviour
    {
        mono.StartCoroutine(StartHighlight(sequences.Last()));
    }

    public static bool CheckEqualPath(TileSegment[] path)
    {
        for (int i = 0; i < sequences.Count; i++)
        {
            //TileSegment[] sequencePath = sequences[i];
            //bool wholeSequence = false;

            //for (int j = 0; j < sequencePath.Length; j++)
            //{
            //    wholeSequence = wholeSequence || path.Contains(sequencePath[j]);
            //}

            if (sequences[i].All(s => path.Contains(s)))
                return true;
        }

        return false;
    }
}
