using Greenyas.Hexagon;
using HexaLinks.Events;
using System.Collections.Generic;
using System.Linq;

namespace HexaLinks.Tile
{
    public class ConnectionCandidates
    {
        private class Pairs
        {
            public List<SideGate> fromGates = new List<SideGate>();
            public List<SideGate> toGates = new List<SideGate>();

            public bool NoConnection => fromGates.Count == 0 || toGates.Count == 0;

            public void Connect()
            {
                foreach(var from in fromGates)
                {
                    foreach(var to in toGates)
                    {
                        SideGate.Connect(from, to);
                    }
                }
            }
        }

        private Dictionary<HexSide.Side, Pairs> candidates = new Dictionary<HexSide.Side, Pairs>();

        public bool AtLeastOneConnection => candidates.Any(c => !c.Value.NoConnection);
        public void AddFromGates(SideGate[] gates, HexSide.Side side)
        {
            //UnityEngine.Debug.Log($"Adding from {gates.Length} gates on {side}");
            for (int i = 0; i < gates.Length; i++)
                AddFromGate(gates[i], side);
        }

        public void AddFromGate(SideGate gate, HexSide.Side side)
        {
            if(!candidates.TryGetValue(side, out Pairs pairs))
                candidates.Add(side, new Pairs());

            candidates[side].fromGates.Add(gate);
        }

        public void AddToGates(SideGate[] gates, HexSide.Side side)
        {
            //UnityEngine.Debug.Log($"Adding to {gates.Length} gates on {side}");
            for (int i = 0; i < gates.Length; i++)
                AddToGate(gates[i], side);
        }

        public void AddToGate(SideGate gate, HexSide.Side side)
        {
            if (!candidates.TryGetValue(side, out Pairs pairs))
                candidates.Add(side, new Pairs());

            candidates[side].toGates.Add(gate);
        }


        public void ConnectPairs(bool sendEvents = true)
        {
            foreach(Pairs pairs in candidates.Values)
            {
                if (pairs.NoConnection && sendEvents)
                {
                    UnityEngine.Debug.Log($"Block side");
                    Events.OnSideBlocked.Call();
                }
                else
                {
                    pairs.Connect();
                    if(sendEvents)
                    {
                        UnityEngine.Debug.Log($"Connected side");
                        Events.OnSideConnected.Call();
                    }
                }
            }
        }

        public static class Events
        {
                public readonly static EventType OnSideConnected = new();
                public readonly static EventType OnSideBlocked = new();
        }
    }


 //   public class ConnectionCandidate
	//{
 //       public readonly SideGate From;
 //       public readonly SideGate[] To;
 //       private static Func<ConnectionCandidate, bool> AtLeastOneConnection => (c) => c.To.Length > 0;

 //       public bool AreValid => AtLeastOneConnection(this);

 //       public ConnectionCandidate(Tile toTile, SideGate fromGate)
	//	{
 //           From = fromGate;
 //           To = toTile.Connectivity.GetAlignedGatesAgainst(fromGate);

 //           ////for (int i = 0; i < possibleGates.Length; i++)
 //           //toSideGates(possibleGates);

 //           //if (possibleGates.Length == 0)
 //           //    UnityEngine.Debug.Log("Blocked!");
 //           //else
 //           //    SideGate.Events.OnSegmentConnected.Call();
 //       }

 //       public void Connect()
 //       {
 //           for(int i = 0; i < To.Length; ++i)
 //               SideGate.Connect(From, To[i]);
 //       }

 //       //private readonly struct ConnectionPair
 //       //{
 //       //    public readonly SideGate fromGate;
 //       //    public readonly SideGate toGate;

 //       //    public ConnectionPair(SideGate gate, SideGate otherGate)
 //       //    {
 //       //        fromGate = gate;
 //       //        toGate = otherGate;
 //       //    }
 //       //}
 //   } 
}
