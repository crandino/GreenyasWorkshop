using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class PathStorage
{
    private static WaitForSeconds timeBetweenHighlight = new WaitForSeconds(0.15f);
    private static WaitForSeconds timeBetweenPaths = new WaitForSeconds(1f);
    private static List<TileSegment[]> sequences = new List<TileSegment[]>();

    static PathStorage()
    {
        sequences.Clear();
    }

    private static IEnumerator StartHighlight()
    {
        for (int i = 0; i < sequences.Count; i++)
        {
            TileSegment[] path = sequences[i];
            for (int j = 0; j < path.Length; j++)
            {
                yield return timeBetweenHighlight;
                path[j].Highlight();
            }

            //yield return timeBetweenPaths;
            //for (int j = 0; j < path.Length; j++)
            //    path[j].Unhighlight();
        }
    }

    public static void AddPath(TileSegment[] path)
    {
        sequences.Add(path);
    }

    public static void RemovePathWithSegments(TileSegment[] segments)
    {
        for (int i = sequences.Count - 1; i >= 0; --i)
        {
            if (sequences[i].Any(s => segments.Contains(s)))
            {
                System.Array.ForEach(sequences[i], s => s.Unhighlight());
                sequences.RemoveAt(i);
            }
        }
    }

    public static void ShowCompletedPaths<T>(T mono) where T : MonoBehaviour
    {
        mono.StartCoroutine(StartHighlight());
    }

    public static bool CheckEqualPath(TileSegment[] path)
    {
        for (int i = 0; i < sequences.Count; i++)
        {
            if (sequences[i].All(s => path.Contains(s)))
                return true;
        }

        return false;
    }
}
