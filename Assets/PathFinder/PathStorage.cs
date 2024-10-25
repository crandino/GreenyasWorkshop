using HexaLinks.Propagation;
using System.Collections.Generic;
using static HexaLinks.PathFinder.PathStorage.Path;

namespace HexaLinks.PathFinder
{
    public static partial class PathStorage
    {
        private readonly static List<PathIterationStep> steps = new List<PathIterationStep>();
        private static PathIterationStep current = null;

        public static void Init(Tile.Tile fromTile)
        {
            current = new PathIterationStep(steps.Count);
            steps.Add(current);

            PathIterator.FindPathsFrom(fromTile);
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