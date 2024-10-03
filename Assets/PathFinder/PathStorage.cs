using Hexalinks.Tile;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hexalinks.PathFinder
{
    public static class PathStorage
    {
        private static List<Path> paths = new List<Path>();

        public static void Clear()
        {
            paths.Clear();
        }

        public static void Add(TileSegment[] segments)
        {
            paths.Add(new Path(segments));
        }

        public class Path
        {
            private readonly struct PathLink
            {
                public readonly TileSegment segment;
                public readonly PlayerOwnership ownership;

                public PathLink(TileSegment segment)
                {
                    this.segment = segment;
                    ownership = segment.GetComponent<PlayerOwnership>();
                }
            }

            private readonly PathLink[] pathLinks;
            private readonly InitialPlayerOwnership startingOwnership;

            public Path(TileSegment[] segments)
            {
                startingOwnership = segments[0].transform.parent.GetComponentInChildren<InitialPlayerOwnership>();
                pathLinks = segments.Select(s => new PathLink(s)).ToArray();

                //PlayerOwnership.OwnershipCounter.Calculate(segments.Select(s => s.GetComponent<PlayerOwnership>()).ToArray());
            }

            public void TriggerContamination()
            {
                foreach (var link in pathLinks)
                {
                    link.ownership.InstantOwnerChange(startingOwnership.Owner);
                }
            }

            public void Log()
            {
                int counter = 1;

                foreach (var link in pathLinks)
                {
                    Debug.Log($"{counter++} - {link.segment.transform.parent.name}");
                }
            }
        }
    }

}