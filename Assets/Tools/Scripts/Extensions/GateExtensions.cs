using Hexalinks.Tile;

namespace Greenyas.Hexagon
{
    public static class GateExtensions
    {
        //public static void Connect(this TileSegment segment, TileSegment to)
        //{

        //}


        public static void Connect(this Gate[] gatesFrom, Gate[] gatesTo)
        {
            for (int i = 0; i < gatesFrom.Length; ++i)
            {
                gatesFrom[i].Connect(gatesTo);                
            }
        }

        public static void Connect(this Gate gateFrom, Gate[] gatesTo)
        {
            for (int i = 0; i < gatesTo.Length; ++i)
            {
                Gate gateTo = gatesTo[i];

                if (gateFrom.IsFacingOtherGate(gateTo))
                {
                    gateFrom.Connect(gateTo);
                    //gateTo.Connections.Add(gateFrom);
                }
            }
        }

        //public static void Disconnect(this Gate gate)
        //{
        //    for(int i = 0; i < gate.Connections.Count; ++i)
        //        gate.Connections[i].Connections.Clear();

        //    gate.Connections.Clear();
        //}

        //public static bool IsFacingOtherGate(this Gate gateFrom, Gate gateTo)
        //{
        //    Assert.IsTrue(gateFrom.Segment != gateTo.Segment);
        //    return gateFrom.node.Side.IsOpposite(gateTo.node.Side);
        //}
    }
}
