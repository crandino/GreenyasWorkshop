using System.Collections.Generic;
using System.Linq;

namespace HexaLinks.Tile
{
    public static class GateExtensions
    {
        public static Gate.ReadOnlyGate[] ToExposedGates(this IList<Gate> gates)
        {
            return gates.Select(g => new Gate.ReadOnlyGate(g)).ToArray();
        }
    } 
}
