using System.Collections.Generic;

namespace HexaLinks.Path.Finder
{
    using Propagation;
    using System.Linq;
    using Tile;
    using UnityEngine;

    //public static partial class PathFinder
    //{
    //    private readonly static List<PathIterationStep> steps = new List<PathIterationStep>();
    //    private static PathIterationStep current = null;

    //    public static void Init(TilePropagator initialTile)
    //    {
    //        current = new PathIterationStep(steps.Count, initialTile);
    //        steps.Add(current);

    //        PathIterator.QueueSearch(initialTile);
    //    }

    //    public static void Reset()
    //    {
    //        steps.Clear();
    //        current = null;
    //    }

    //    public static void Add(Path newPath)
    //    {
    //        if (steps.SkipLast(1).Any(s => s.Contains(newPath.HashID)))
    //        {
    //            Debug.Log($"On step {current.Id}, already exists {newPath}");
    //            return;
    //        }

    //        current.Add(newPath);
    //        Debug.Log($"On step {current.Id}, added {newPath}");
    //    }

    //    public static void StartPropagation()
    //    {
    //        Game.Instance.GetSystem<PropagationManager>().TriggerPropagation(current);
    //    }       
    //}
}