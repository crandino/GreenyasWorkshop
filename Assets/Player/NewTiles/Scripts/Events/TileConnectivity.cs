using Greenyas.Hexagon;
using System.Collections.Generic;
using UnityEngine;
using static Hexalinks.Tile.Node;
using static Hexalinks.Tile.Tile;

namespace Hexalinks.Tile
{
    public class TileConnectivity : TileEvent
    {
        [SerializeField]
        private TileSegment[] segments = null;

        public Node.Gate[] Gates
        {
            get
            {
                List<Node.Gate> gates = new List<Gate>();

                for (int i = 0; i < segments.Length; i++)
                    gates.AddRange(segments[i].Gates);

                return gates.ToArray();
            }
        }

        public override void Initialize()
        {
            for (int i = 0; i < segments.Length; ++i)
                segments[i].Initialize();
        }

        public override void OnPickUp(Tile.Data data)
        {
            DisconnectTile();
        }

        public override void OnRelease(Tile.Data data)
        {
            ConnectTile(data);
        }

        private void ConnectTile(Data data)
        {
            for (int i = 0; i < Gates.Length; ++i)
            {
                Gate from = Gates[i];
                CubeCoord neighborHexCoord = data.Coord + CubeCoord.GetToNeighborCoord(from.Node.Side);

                if (HexMap.Instance.TryGetTile(neighborHexCoord, out Tile.Data neighborTileData))
                    from.Connect(neighborTileData.Gates);
            }
        }


        private void DisconnectTile()
        {
            for (int i = 0; i < Gates.Length; ++i)
                Gates[i].Disconnect();
        }

        //public List<TileSegment.Gate> GetAllGates()
        //{
        //    List<TileSegment.Gate> gates = new List<TileSegment.Gate>();

        //    for (int i = 0; i < segments.Length; i++)
        //        segments[i].GetAllGates(gates);

        //    return gates;
        //}

        //private Node[] GetAllNodes()
        //{
        //    List<Node> nodes = new();

        //    for (int i = 0; i < segments.Length; i++)
        //        segments[i].GetAllNodes(nodes);

        //    return nodes.ToArray();
        //}

        //private readonly static List<TileSegment.Gate> gates = new List<TileSegment.Gate>();

        //public bool SearchGatesAgainst(HexSide.Side side, out List<TileSegment.Gate> gates)
        //{
        //    gates = new List<TileSegment.Gate>();

        //    for (int i = 0; i < segments.Length; i++)
        //        segments[i].SearchGatesAgainst(side, gates);

        //    return gates.Count != 0;
        //}

        //public List<TileSegment.Gate> GetAllGates()
        //{
        //    gates.Clear();

        //    for (int i = 0; i < segments.Length; i++)
        //        segments[i].GetAllGates(gates);

        //    return gates;
        //}
    }

}