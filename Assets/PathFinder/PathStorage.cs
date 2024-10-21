using Hexalinks.Tile;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Hexalinks.PathFinder.PathStorage.Path;

namespace Hexalinks.PathFinder
{
    public static class PathStorage
    {
        private class PropagationStep
        {
            public int id;
            public List<Path> paths;

            public PropagationStep(int id)
            {
                this.id = id;
                paths = new List<Path>();
            }

            public List<Link[]> UnifyPaths()
            {
                if (paths.Count == 0)
                    return null;

                int longestPath = paths.Max(p => p.Links.Length);
                List<Link>[] upath = new List<Link>[longestPath];

                for (int i = 0; i < upath.Length; ++i)
                {
                    upath[i] = new List<Link>();
                }

                for (int i = 0; i < paths.Count; ++i)
                {
                    for (int j = 0; j < paths[i].Links.Length; ++j)
                    {
                        upath[j].Add(paths[i].Links[j]);
                    }
                }

                return upath.Select(p => p.Distinct(new PathLinkComparer()).ToArray()).ToList();
            }

            public class LinkGroup
            {
                private readonly Link[] links;
                public bool IsAlone => links.Length == 1;

                //public Link[] GetOppositeLinks()
                //{
                //    links.Sele
                //}
            }

            class PathLinkComparer : IEqualityComparer<Link>
            {
                public bool Equals(Link x, Link y)
                {
                    return x.Ownership == y.Ownership && x.ForwardTraversal == y.ForwardTraversal;
                }

                public int GetHashCode(Link product)
                {
                    return product.Ownership.GetHashCode();
                }
            }
        }

        private readonly static List<PropagationStep> steps = new List<PropagationStep>();
        private static PropagationStep current = null;

        private readonly static List<uint> oldPathsHashes = new();

        public static void InitNewPropagation(bool fullReset = false)
        {
            if (fullReset)
                ResetPropagationSteps();

            current = new PropagationStep(steps.Count);
            steps.Add(current);
        }

        private static void ResetPropagationSteps()
        {
            steps.Clear();
            current = null;
        }

        public static void Add(Path newPath)
        {
            if(!oldPathsHashes.Contains(newPath.HashID))
            {
                current.paths.Add(newPath);
                oldPathsHashes.Add(newPath.HashID);
                newPath.Log();
            }
            else
            {
                Path identicPath = current.paths.First(p => p.HashID == newPath.HashID);
                //int midPointLink = Mathf.CeilToInt((float)identicPath.Links.Length / 2);

                oldPathsHashes.Remove(identicPath.HashID);
                identicPath.Merge(newPath);

                identicPath.Log();
                newPath.Log();

                oldPathsHashes.Add(identicPath.HashID);
                oldPathsHashes.Add(newPath.HashID);
                current.paths.Add(newPath);
            }
        }

        public static void StartPropagation()
        {
            List<Link[]> unifiedPath = current.UnifyPaths();

            if(unifiedPath != null)
                OwnershipPropagation.Start(unifiedPath);
        }

        public class Path
        {
            private uint hash = InvalidHash;

            private const uint InvalidHash = uint.MaxValue;

            public uint HashID
            {
                get
                {
                    if(hash == InvalidHash)
                        Array.ForEach(Links, link => hash += link.Hash);
                    return hash;
                }
            }

            public Link[] Links { private set; get; }

            public readonly struct Link 
            {
                private readonly Gate.ExposedGate entryGate;

                public readonly PlayerOwnership Ownership => entryGate.Ownership;
                public readonly uint Hash => entryGate.Hash;
                public readonly bool? ForwardTraversal => forwardTraversal;

                private readonly bool? forwardTraversal;

                public Link(Gate.ExposedGate gate)
                {
                    entryGate = gate;
                    forwardTraversal = entryGate.ForwardTraversalDir;
                }

                public Link(Link copyLink)
                {
                    entryGate = copyLink.entryGate;
                    forwardTraversal = null;
                }
            }

            private struct OwnershipCounter
            {
                public class OwnershipSegments
                {
                    private readonly Link startSegment;
                    private int counter;
                    public PlayerOwnership.Ownership SegmentOwner => startSegment.Ownership.Owner;

                    public OwnershipSegments(Link startSegment)
                    {
                        this.startSegment = startSegment;
                        counter = 1;
                    }

                    public void Increment()
                    {
                        ++counter;
                    }
                }

                public List<OwnershipSegments> segments;

                public static OwnershipCounter Calculate(Link[] links)
                {
                    OwnershipCounter counter;
                    counter.segments = new List<OwnershipSegments>();

                    foreach (var l in links)
                    {
                        if (counter.segments.Count == 0 || counter.segments.Last().SegmentOwner != l.Ownership.Owner)
                            counter.segments.Add(new OwnershipSegments(l));
                        else
                            counter.segments[^1].Increment();
                    }

                    return counter;
                }
            }
          
            public Path(Gate.ExposedGate[] gates)
            {
                Links = gates.Select(s => new Link(s)).ToArray();
                OwnershipCounter counter = OwnershipCounter.Calculate(Links);               
            }

            public void Merge(Path otherPath)
            {
                hash = InvalidHash;

                int midPointLink = Mathf.CeilToInt((float)Links.Length / 2);
                Links = Links.Take(midPointLink).ToArray();
                Links[midPointLink - 1] = new Link(Links[midPointLink - 1]);
                otherPath.Links = otherPath.Links.Take(midPointLink).ToArray();
                otherPath.Links[midPointLink - 1] = new Link(otherPath.Links[midPointLink - 1]);
            }

            public void Log()
            {
                int counter = 1;
                string text = $"Path ID: {HashID} ({Links.Length} segments)\n";

                foreach (var link in Links)
                    text += $"{counter++} - {link.Ownership.transform.parent.name}:{link.Ownership.name}\n";

                Debug.Log(text);
            }      
        }
    }

}