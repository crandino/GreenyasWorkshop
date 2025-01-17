using HexaLinks.Ownership;
using HexaLinks.Tile;
using HexaLinks.Turn;
using System.Collections.Generic;
using System.Linq;
using static HexaLinks.Tile.Gate;

namespace HexaLinks.Path.Finder
{
    public static partial class PathFinder
    {
        public class PathIterationStep
        {
            public TilePropagator InitialPropagator { private set; get; }
            public ReadOnlyGate[][] CombinedPaths { private set; get; }
            public int MaxLengthPath => paths.Max(p => p.links.Length);
            private bool ContainPaths => paths.Count > 0;

            private readonly List<Path> paths = new List<Path>();

            public PathIterationStep(TilePropagator propagator)
            {
                InitialPropagator = propagator;
            }

            public void AddPath(Path path)
            {
                if (path.IsFullyControlledBy(TurnManager.CurrentPlayer))
                    return;

                UnityEngine.Debug.Log($"Added {path}");
                paths.Add(path);
            }

            public bool ExistsPropagation()
            {
                Owner propagationOwner = TurnManager.CurrentPlayer;
                return ContainPaths && paths.Any(p => p.links.Any(l => !l.Ownership.IsSameOwner(propagationOwner)));
            }

            public void Combine()
            {
                CombinedPaths = new ReadOnlyGate[MaxLengthPath][];

                for (int i = 0; i < CombinedPaths.Length; ++i)
                {
                    List<ReadOnlyGate> gatesCombined = new List<ReadOnlyGate>();

                    foreach (var path in paths)
                    {
                        if (i < path.links.Length && !gatesCombined.Contains(path.links[i]))
                            gatesCombined.Add(path.links[i]);
                    }

                    CombinedPaths[i] = gatesCombined.ToArray();
                }
            }
        }
    }
}