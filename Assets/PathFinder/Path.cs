using HexaLinks.Ownership;
using HexaLinks.Tile;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HexaLinks.Path.Finder
{
    public static partial class PathFinder
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
                public class OwnershipSegments
                {
                    private readonly Link[] links;
                    public Range Range { private set; get; }
                    public int GetNumberOfLinks() => Range.GetOffsetAndLength(links.Length).Length;

                    public PlayerOwnership.Ownership GetOwner() => links[Range.Start].Ownership.Owner;

                    public OwnershipSegments(Link[] pathLinks, Index startIndex)
                    {
                        links = pathLinks;
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
                    segments = new List<OwnershipSegments>();

                    Link[] linksMinusFirst = links.Skip(1).ToArray();

                    int index = 1;

                    foreach (Link link in linksMinusFirst)
                    {
                        if (segments.Count == 0 || segments.Last().GetOwner() != link.Ownership.Owner)
                        {
                            if (segments.LastOrDefault() != null)
                                segments.Last().CloseRange(index);

                            segments.Add(new OwnershipSegments(links, index));
                        }
                        ++index;
                    }
                }

                public readonly Range CalculateOwnershipRange(PlayerOwnership.Ownership initialOwner)
                {
                    int totalOwnershipCounter = 1;

                    foreach (OwnershipSegments seg in segments)
                    {
                        PlayerOwnership.Ownership segmentOwner = seg.GetOwner();

                        if (segmentOwner == PlayerOwnership.Ownership.None ||
                            initialOwner != segmentOwner && totalOwnershipCounter > seg.GetNumberOfLinks())
                        {
                            totalOwnershipCounter += seg.GetNumberOfLinks();
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
                PropagationRange = counter.CalculateOwnershipRange(((InitialPlayerOwnership)StartLink.Ownership).Owner);
            }

            //https://stackoverflow.com/questions/2978311/format-a-string-into-columns
            public override string ToString()
            {
                int counter = 0;
                string text = $"Path ID: {HashID} ({Links.Length} segments)\n";      

                foreach (var link in Links)
                    text += string.Format("{0,-3}|{1,-40}|{2,-15}|{3,-12}\n", ++counter, link.Ownership.transform.parent.name, link.Ownership.name,link.Ownership.Owner);

                return text;
            }
        }
    }
    
}
