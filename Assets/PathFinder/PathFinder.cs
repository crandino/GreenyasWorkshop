using System.Collections.Generic;

namespace HexaLinks.Path.Finder
{
    using Propagation;
    using Tile;
    using static Path.Finder.PathFinder.Path;

    public static partial class PathFinder
    {
        private readonly static List<PathIterationStep> steps = new List<PathIterationStep>();
        private static PathIterationStep current = null;

        public static void Init(TilePropagator initialTile)
        {
            current = new PathIterationStep(steps.Count, initialTile);
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
            Game.Instance.GetSystem<PropagationManager>().Start(current);
        }       
    }
}