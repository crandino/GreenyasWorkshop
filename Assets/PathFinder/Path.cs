using HexaLinks.Tile;
using System;
using System.Linq;

namespace HexaLinks.Path.Finder
{
    public static partial class PathFinder
    {
        public class Path
        {
            private uint hash = InvalidHash;
            private const uint InvalidHash = 0;

            public uint HashID
            {
                get
                {
                    if (hash == InvalidHash)
                    {
                        hash = 0;
                        Array.ForEach(links, link => hash += link.Hash);
                    }
                    return hash;
                }
            }

            public readonly Gate.ReadOnlyGate[] links;

            public Path(Gate.ReadOnlyGate[] gates)
            {
                links = gates;              
            }

            public bool IsFullyControlledBy(Ownership.Owner owner)
            {
                return links.All(l => l.Ownership.IsSameOwner(owner));
            }

            //https://stackoverflow.com/questions/2978311/format-a-string-into-columns
            public override string ToString()
            {
                int counter = 0;
                string text = $"Path ID: {HashID} ({links.Length} segments)\n";      

                foreach (var link in links)
                    text += string.Format("{0,-3}|{1,-40}|{2,-15}|{3,-12}\n", ++counter, link.Ownership.transform.parent.name, link.Ownership.name,link.Ownership.Owner);

                return text;
            }
        }
    }
    
}
