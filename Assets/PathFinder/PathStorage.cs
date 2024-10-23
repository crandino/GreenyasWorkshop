using Hexalinks.Tile;
using HexaLinks.Ownership;
using HexaLinks.Propagation;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Hexalinks.PathFinder.PathStorage.Path;

namespace Hexalinks.PathFinder
{
    public static class PathStorage
    {
        private class Storage
        {
            private readonly static List<uint> pathsHashes = new();

            public bool ExistsPath(uint hash) => pathsHashes.Contains(hash);
            public void AddPath(uint hash) => pathsHashes.Add(hash);
            public void RemovePath(uint hash) => pathsHashes.Remove(hash);
            public void Clear() => pathsHashes.Clear();
        }

        private readonly static Storage storage = new Storage();

        private class PropagationStep
        {
            private readonly int id;
            private readonly Storage storage;
            private List<Path> paths;

            public PropagationStep(int id)
            {
                this.id = id;
                paths = new List<Path>();
                storage = PathStorage.storage;
            }

            public void AddPath(Path newPath)
            {
                if(storage.ExistsPath(newPath.HashID))
                {
                    Path identicPath = current.paths.First(p => p.HashID == newPath.HashID);

                    storage.RemovePath(identicPath.HashID);
                    identicPath.Log("Before trimmed");
                    Merge(newPath, identicPath);
                    identicPath.Log("After trimmed");

                    storage.AddPath(identicPath.HashID);
                    current.paths.Add(newPath);
                }

                storage.AddPath(newPath.HashID);
                paths.Add(newPath);
                newPath.Log("Added");
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

            class PathLinkComparer : IEqualityComparer<Link>
            {
                public bool Equals(Link x, Link y)
                {
                    return x.Ownership == y.Ownership && x.ForwardPropagation == y.ForwardPropagation;
                }

                public int GetHashCode(Link product)
                {
                    return product.Ownership.GetHashCode();
                }
            }
        }

        private readonly static List<PropagationStep> steps = new List<PropagationStep>();
        private static PropagationStep current = null;

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
            current.AddPath(newPath);
        }

        public static void StartPropagation()
        {
            List<Link[]> unifiedPath = current.UnifyPaths();

            if(unifiedPath != null)
                PropagationManager.Start(unifiedPath);
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
                public readonly bool ForwardPropagation => entryGate.ForwardTraversalDir;

                public Link(Gate.ExposedGate gate)
                {
                    entryGate = gate;
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

            public static void Merge(Path path1, Path path2)
            {
                path1.hash = path2.hash = InvalidHash;

                int midPointLink = Mathf.CeilToInt((float)path1.Links.Length / 2);
                path1.Links = path1.Links.Take(midPointLink).ToArray();
                path2.Links = path2.Links.Take(midPointLink).ToArray();
            }

            private string LogMessage 
            {
                get
                {
                    int counter = 1;
                    string text = $"Path ID: {HashID} ({Links.Length} segments)\n";

                    foreach (var link in Links)
                        text += $"{counter++} - {link.Ownership.transform.parent.name}:{link.Ownership.name}\n";

                    return text;
                }
            }     
            
            public void Log(string headerMessage)
            {
                Debug.Log($"{headerMessage} {LogMessage}");
            }
        }
    }

}