using HexaLinks.Propagation;
using HexaLinks.Tile;
using System.Collections.Generic;
using static HexaLinks.Path.Finder.PathFinder.Path;

namespace HexaLinks.Path.Finder
{
    public static partial class PathFinder
    {
        private readonly static List<PathIterationStep> steps = new List<PathIterationStep>();
        private static PathIterationStep current = null;

        public static void Init(TilePropagator initialTile)
        {
            current = new PathIterationStep(steps.Count);
            steps.Add(current);

            PathIterator.FindPathsFrom(initialTile);
        }

        public static void Reset()
        {
            steps.Clear();
            current = null;
        }

        public static void Add(Path newPath)
        {
            current.Add(newPath);           
        }

        public static void StartPropagation()
        {
            List<Link[]> unifiedPath = current.UnifyPaths();

            if (unifiedPath != null)
                PropagationManager.Start(unifiedPath);
        }       
    }
}