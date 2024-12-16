using HexaLinks.Tile;
using System.Collections.Generic;
using System.Linq;
using static HexaLinks.Tile.Gate;

namespace HexaLinks.Path.Finder
{
    public static partial class PathFinder
    {
        public class PathIterationStep
        {
            public TilePropagator Precursor { private set; get; }
            public ReadOnlyGate[][] CombinedPaths { private set; get; }
            public int MaxLengthPath => paths.Max(p => p.links.Length);
            public bool PathsFound => paths != null && paths.Count > 0;

            private readonly List<Path> paths = new List<Path>();

            public PathIterationStep(TilePropagator tile)
            {
                UnityEngine.Debug.Log($"New precursor added: {tile}");
                Precursor = tile;
            }

            public void AddPath(Path path)
            {
                UnityEngine.Debug.Log($"Added {path}");
                paths.Add(path);
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