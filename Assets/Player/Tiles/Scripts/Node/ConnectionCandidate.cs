using System;
using System.Collections.Generic;

namespace HexaLinks.Tile
{
    public class ConnectionCandidate
	{
        private readonly List<ConnectionPair> connectionPairs = new List<ConnectionPair>();
        private static Func<ConnectionCandidate, bool> AtLeastOneConnection => (c) => Game.Instance.GetSystem<HexMap>().NumOfTiles == 0 || c.connectionPairs.Count > 0;

        public bool AreValid => AtLeastOneConnection(this);

        public ConnectionCandidate(Tile toTile, SideGate fromGate)
		{
            SideGate[] possibleGates = toTile.Connectivity.GetAlignedGatesAgainst(fromGate);

            for (int i = 0; i < possibleGates.Length; i++)
                connectionPairs.Add(new(fromGate, possibleGates[i]));
        }

        public void Connect()
        {
            for(int i = 0; i < connectionPairs.Count; ++i)
                SideGate.Connect(connectionPairs[i].fromGate, connectionPairs[i].toGate);
        }

        private readonly struct ConnectionPair
        {
            public readonly SideGate fromGate;
            public readonly SideGate toGate;

            public ConnectionPair(SideGate gate, SideGate otherGate)
            {
                fromGate = gate;
                toGate = otherGate;
            }
        }
    } 
}
