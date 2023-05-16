using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class PathStorage
{
    private static WaitForSeconds timeBetweenHighlight = new WaitForSeconds(0.15f);
    private static WaitForSeconds timeBetweenPaths = new WaitForSeconds(1f);

    private static List<Path> paths = new List<Path>();
    private static Dictionary<TileSegment, int> segmentShared = new Dictionary<TileSegment, int>();

    private class Path
    {
        public bool Shown { private set; get; }
        private TileSegment[] path;

        public Path(TileSegment[] path)
        {
            Shown = false;
            this.path = path;

            Array.ForEach(path, s =>
            {
                if (!segmentShared.ContainsKey(s))
                    segmentShared.Add(s, 0);
            });
        }

        public bool Contains(TileSegment[] segments)
        {
            return path.Any(s => segments.Contains(s));
        }

        public bool ContainsExactly(TileSegment[] segments)
        {
            return path.Length == segments.Length && path.All(s => segments.Contains(s));
        }

        private void LogSharedSegments()
        {
            foreach (var item in segmentShared)
            {
                Debug.Log($"Segment {item.Key}, {item.Value}");
            }
        }

        public void Show()
        {
            void Highlight(TileSegment segment)
            {
                segmentShared[segment]++;
                segment.Highlight();
            }

            Shown = true;
            Array.ForEach(path, s => Highlight(s));
            LogSharedSegments();
        }

        public void Unshow()
        {
            void Unhighlight(TileSegment segment)
            {
                segmentShared[segment]--;
                if (segmentShared[segment] == 0)
                {
                    segment.Unhighlight();
                    segmentShared.Remove(segment);
                }
            }

            //Shown = false;
            Array.ForEach(path, s => Unhighlight(s));
            LogSharedSegments();

        }

        //public static bool operator ==(Path p1, Path p2)
        //{
        //    return p1.path.Length == p2.path.Length &&
        //           p1.path.All(s => p2.path.Contains(s));
        //}

        //public static bool operator !=(Path p1, Path p2)
        //{
        //    return p1.path.Length != p2.path.Length ||
        //           p1.path.Any(s => !p2.path.Contains(s));
        //}
    }

    static PathStorage()
    {
        paths.Clear();
        segmentShared.Clear();
    }

    public static void AddPath(TileSegment[] path)
    {
        for (int i = 0; i < paths.Count; i++)
            if (paths[i].ContainsExactly(path))
                return;

        paths.Add(new Path(path));
    }

    public static void RemovePath(TileSegment[] segments)
    {
        for (int i = paths.Count - 1; i >= 0; --i)
        {
            if (paths[i].Contains(segments))
            {
                paths[i].Unshow();
                paths.RemoveAt(i);
            }
        }
    }

    public static void ShowCompletedPaths()
    {
        paths.ForEach(p =>
        {
            if (!p.Shown)
                p.Show();
        });
    }
}
