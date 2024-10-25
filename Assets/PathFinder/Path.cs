using HexaLinks.Ownership;
using HexaLinks.Tile;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HexaLinks.PathFinder
{
    public static partial class PathStorage
    {
        public class Path
        {
            private uint hash = InvalidHash;
            private const uint InvalidHash = int.MaxValue;

            public uint HashID
            {
                get
                {
                    if (hash == InvalidHash)
                    {
                        hash = 0;
                        Array.ForEach(Links, link => hash += link.Hash);
                    }
                    return hash;
                }
            }

            public Link StartLink { private set; get; }
            public Range PropagationRange { private set; get; }
            public Link[] Links { private set; get; }

            //TODO: Split all of these. Maybe, add a reference on the Ownership counter to encapsulate better and not to pass any other possible array of Links

            public readonly struct Link
            {
                private readonly Gate.ExposedGate gate;

                public Link(Gate.ExposedGate entryGate)
                {
                    gate = entryGate;
                }

                public readonly PlayerOwnership Ownership => gate.Ownership;
                public readonly uint Hash => gate.Hash;
                public readonly bool ForwardTraversal => gate.ForwardTraversalDir;

                public static Link Empty = new Link();
            }

            private struct OwnershipCounter
            {
                private readonly Link[] pathLinks;

                public class OwnershipSegments
                {
                    public Range Range { private set; get; }
                    public int GetNumberOfLinks(Link[] links) => Range.GetOffsetAndLength(links.Length).Length;

                    public PlayerOwnership.Ownership GetOwner(Link[] links) => links[Range.Start].Ownership.Owner;

                    public OwnershipSegments(Index startIndex)
                    {
                        Range = Range.StartAt(startIndex);
                    }

                    public void CloseRange(int endIndex)
                    {
                        Range = new Range(Range.Start, endIndex);
                    }
                }

                private readonly List<OwnershipSegments> segments;

                public OwnershipCounter(Link[] links)
                {
                    pathLinks = links;
                    segments = new List<OwnershipSegments>();

                    links = links.Skip(1).ToArray();

                    int index = 1;

                    foreach (var l in links)
                    {
                        if (segments.Count == 0 || segments.Last().GetOwner(links) != l.Ownership.Owner)
                        {
                            if (segments.LastOrDefault() != null)
                                segments.Last().CloseRange(index);

                            segments.Add(new OwnershipSegments(index));
                        }
                        ++index;
                    }
                }

                public readonly Range CalculateOwnershipRange(PlayerOwnership.Ownership initialOwner)
                {
                    int totalOwnershipCounter = 1;

                    foreach (OwnershipSegments seg in segments)
                    {
                        PlayerOwnership.Ownership segmentOwner = seg.GetOwner(pathLinks);

                        if (segmentOwner == PlayerOwnership.Ownership.None ||
                            initialOwner != segmentOwner && totalOwnershipCounter > seg.GetNumberOfLinks(pathLinks))
                        {
                            totalOwnershipCounter += seg.GetNumberOfLinks(pathLinks);
                        }
                        else
                            return Range.EndAt(seg.Range.Start);
                    }

                    return Range.EndAt(^0);
                }
            }

            public Path(Gate.ExposedGate[] gates)
            {
                Links = gates.Select(s => new Link(s)).ToArray();
                StartLink = Links.First();
                OwnershipCounter counter = new OwnershipCounter(Links);
                PropagationRange = counter.CalculateOwnershipRange(StartLink.Ownership.Owner);
            }

            public override string ToString()
            {
                int counter = 1;
                string text = $"Path ID: {HashID} ({Links.Length} segments)\n";

                foreach (var link in Links)
                    text += $"{counter++} - {link.Ownership.transform.parent.name}:{link.Ownership.name}\n";

                return text;
            }
        }
    }
    
}
