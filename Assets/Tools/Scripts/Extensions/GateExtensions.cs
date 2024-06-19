using Greenyas.Hexagon;
using Hexalinks.Tile;
using UnityEngine;

namespace Greenyas.Hexagon
{
    public static class GateExtensions
    {
        public static void Connect(this Node.Gate[] gatesFrom, Node.Gate[] gatesTo)
        {
            for (int i = 0; i < gatesFrom.Length; ++i)
            {
                gatesFrom[i].Connect(gatesTo);

                //for (int j = 0; j < gatesTo.Length; ++j)
                //{
                //    Node.Gate from = gatesFrom[i], to = gatesTo[j];

                //    if (from.Node.Side.IsOpposite(to.Node.Side))
                //    {
                //        from.Node.Connections.Add(to);
                //        to.Node.Connections.Add(from);
                //    }
                //}
            }
        }

        public static void Connect(this Node.Gate gateFrom, Node.Gate[] gatesTo)
        {
            for (int i = 0; i < gatesTo.Length; ++i)
            {
                Node.Gate to = gatesTo[i];

                if (gateFrom.Node.Side.IsOpposite(to.Node.Side))
                {
                    gateFrom.Node.Connections.Add(to);
                    to.Node.Connections.Add(gateFrom);
                }
            }
        }

        public static void Disconnect(this Node.Gate gate)
        {
            for(int i = 0; i < gate.Node.Connections.Count; ++i)
            {
                gate.Node.Connections[i].Node.Connections.Clear();
            }
            gate.Node.Connections.Clear();
        }
    }
}
